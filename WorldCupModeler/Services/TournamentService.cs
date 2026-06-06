using WorldCupModeler.Data;
using WorldCupModeler.Models;

namespace WorldCupModeler.Services;

/// <summary>
/// Holds the tournament state in memory, computes standings / bracket resolution,
/// and persists to localStorage. Registered as a singleton for the WASM app.
/// </summary>
public class TournamentService(LocalStorageService storage)
{
    private const string StorageKey = "worldcup-2026-state";

    private TournamentState _state = new();
    private bool _loaded;

    /// <summary>Raised whenever the state changes so the UI can refresh.</summary>
    public event Action? OnChange;

    public IReadOnlyList<string> Groups => Teams2026.Groups;
    public IReadOnlyList<Team> Teams => Teams2026.All;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        _loaded = true;
        var saved = await storage.GetAsync<TournamentState>(StorageKey);
        if (saved is not null) _state = saved;
    }

    private async Task PersistAsync()
    {
        await storage.SetAsync(StorageKey, _state);
        OnChange?.Invoke();
    }

    // ----- Group stage -----------------------------------------------------

    public IReadOnlyList<GroupFixture> GroupFixtures(string group) => Bracket2026.GroupFixtures(group);

    public MatchScore GetGroupScore(string matchId) =>
        _state.GroupResults.TryGetValue(matchId, out var s) ? s : new MatchScore();

    public async Task SetGroupScoreAsync(string matchId, int? home, int? away)
    {
        if (home is null && away is null)
            _state.GroupResults.Remove(matchId);
        else
            _state.GroupResults[matchId] = new MatchScore { HomeScore = home, AwayScore = away };
        await PersistAsync();
    }

    /// <summary>Updates a single side of a group score, reading the latest stored value first.</summary>
    public async Task SetGroupScoreSideAsync(string matchId, int? value, bool isHome)
    {
        var current = GetGroupScore(matchId);
        var home = isHome ? value : current.HomeScore;
        var away = isHome ? current.AwayScore : value;
        await SetGroupScoreAsync(matchId, home, away);
    }

    private Dictionary<string, Standing> ComputeRawStandings(string group)
    {
        var teams = Teams2026.InGroup(group).ToList();
        var table = teams.ToDictionary(t => t.Id, t => new Standing { Team = t });

        foreach (var fx in Bracket2026.GroupFixtures(group))
        {
            var score = GetGroupScore(fx.MatchId);
            if (score.HomeScore is not int h || score.AwayScore is not int a) continue;

            var home = table[fx.HomeTeamId];
            var away = table[fx.AwayTeamId];
            home.Played++; away.Played++;
            home.GoalsFor += h; home.GoalsAgainst += a;
            away.GoalsFor += a; away.GoalsAgainst += h;
            if (h > a) { home.Won++; away.Lost++; }
            else if (h < a) { away.Won++; home.Lost++; }
            else { home.Drawn++; away.Drawn++; }
        }
        return table;
    }

    /// <summary>Default comparison: points, goal difference, goals for, then seeded order.</summary>
    private static IEnumerable<Standing> SortDefault(IEnumerable<Standing> standings, List<string> seedOrder) =>
        standings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .ThenBy(s => seedOrder.IndexOf(s.Team.Id));

    public IReadOnlyList<Standing> GetStandings(string group)
    {
        var raw = ComputeRawStandings(group);
        var seedOrder = Teams2026.InGroup(group).Select(t => t.Id).ToList();

        List<Standing> ordered;
        if (_state.GroupOrderOverrides.TryGetValue(group, out var ov) &&
            ov.Count == raw.Count && ov.All(raw.ContainsKey))
        {
            ordered = ov.Select(id => raw[id]).ToList();
        }
        else
        {
            ordered = SortDefault(raw.Values, seedOrder).ToList();
        }

        for (var i = 0; i < ordered.Count; i++) ordered[i].Position = i + 1;
        return ordered;
    }

    public bool HasOverride(string group) => _state.GroupOrderOverrides.ContainsKey(group);

    public async Task MoveTeamAsync(string group, string teamId, int direction)
    {
        var order = GetStandings(group).Select(s => s.Team.Id).ToList();
        var idx = order.IndexOf(teamId);
        var target = idx + direction;
        if (idx < 0 || target < 0 || target >= order.Count) return;
        (order[idx], order[target]) = (order[target], order[idx]);
        _state.GroupOrderOverrides[group] = order;
        await PersistAsync();
    }

    public async Task ClearOverrideAsync(string group)
    {
        if (_state.GroupOrderOverrides.Remove(group))
            await PersistAsync();
    }

    public bool IsGroupComplete(string group) =>
        Bracket2026.GroupFixtures(group).All(fx =>
        {
            var s = GetGroupScore(fx.MatchId);
            return s.HomeScore is not null && s.AwayScore is not null;
        });

    // ----- Third-place ranking --------------------------------------------

    public IReadOnlyList<ThirdPlaceEntry> GetThirdPlaceRanking()
    {
        var thirds = Groups
            .Select(g => GetStandings(g).First(s => s.Position == 3))
            .Select(s => new ThirdPlaceEntry { Standing = s, Group = s.Team.Group })
            .ToList();

        var ranked = thirds
            .OrderByDescending(e => e.Standing.Points)
            .ThenByDescending(e => e.Standing.GoalDifference)
            .ThenByDescending(e => e.Standing.GoalsFor)
            .ThenBy(e => e.Group)
            .ToList();

        for (var i = 0; i < ranked.Count; i++)
        {
            ranked[i].Rank = i + 1;
            ranked[i].Qualified = i < 8;
        }
        return ranked;
    }

    /// <summary>
    /// Assigns the eight best third-placed teams to the eight group-winner slots, never
    /// matching a team against a winner from its own group. This is a deterministic
    /// assignment (a simplification of FIFA's official Annex C lookup table).
    /// </summary>
    private Dictionary<string, Team> ComputeThirdPlaceAssignment()
    {
        var winnerGroups = Bracket2026.ThirdPlaceWinnerGroups.ToList();
        var candidates = GetThirdPlaceRanking()
            .Where(e => e.Qualified)
            .Select(e => e.Standing.Team)
            .ToList();

        var result = new Dictionary<string, Team>();
        if (candidates.Count != winnerGroups.Count) return result;

        var used = new bool[candidates.Count];

        bool Assign(int slot)
        {
            if (slot == winnerGroups.Count) return true;
            for (var i = 0; i < candidates.Count; i++)
            {
                if (used[i] || candidates[i].Group == winnerGroups[slot]) continue;
                used[i] = true;
                result[winnerGroups[slot]] = candidates[i];
                if (Assign(slot + 1)) return true;
                used[i] = false;
                result.Remove(winnerGroups[slot]);
            }
            return false;
        }

        Assign(0);
        return result;
    }

    // ----- Knockout --------------------------------------------------------

    /// <summary>Resolves every knockout match's teams and applies stored results in order.</summary>
    public IReadOnlyList<ResolvedKnockoutMatch> GetKnockoutMatches()
    {
        var standings = Groups.ToDictionary(g => g, GetStandings);
        var thirds = ComputeThirdPlaceAssignment();
        var resolved = new Dictionary<string, ResolvedKnockoutMatch>();

        ResolvedSlot Resolve(SlotSource src)
        {
            switch (src.Kind)
            {
                case SlotSourceKind.GroupPosition:
                    var row = standings[src.Group!].FirstOrDefault(s => s.Position == src.Position);
                    return new ResolvedSlot(row?.Team, $"{src.Position}{src.Group}");
                case SlotSourceKind.ThirdPlace:
                    thirds.TryGetValue(src.Group!, out var third);
                    return new ResolvedSlot(third, "3rd place");
                case SlotSourceKind.MatchWinner:
                    return new ResolvedSlot(resolved[src.MatchId!].Winner, $"Winner {src.MatchId}");
                case SlotSourceKind.MatchLoser:
                    return new ResolvedSlot(resolved[src.MatchId!].Loser, $"Loser {src.MatchId}");
                default:
                    return new ResolvedSlot(null, "?");
            }
        }

        foreach (var fx in Bracket2026.KnockoutFixtures)
        {
            var home = Resolve(fx.Home);
            var away = Resolve(fx.Away);
            var match = new ResolvedKnockoutMatch { Fixture = fx, Home = home, Away = away };

            if (_state.KnockoutResults.TryGetValue(fx.MatchId, out var rs) &&
                SamePairing(rs.HomeTeamId, rs.AwayTeamId, home.Team?.Id, away.Team?.Id))
            {
                // Pairing is unchanged, so the stored result still belongs to this match.
                match.HomeScore = rs.HomeScore;
                match.AwayScore = rs.AwayScore;
                match.DecidedByPenalties = rs.DecidedByPenalties;
                if (rs.WinnerTeamId == home.Team?.Id || rs.WinnerTeamId == away.Team?.Id)
                    match.WinnerTeamId = rs.WinnerTeamId;
            }

            resolved[fx.MatchId] = match;
        }

        return Bracket2026.KnockoutFixtures.Select(f => resolved[f.MatchId]).ToList();
    }

    private static bool SamePairing(string? aHome, string? aAway, string? bHome, string? bAway)
    {
        static string Key(string? x, string? y)
        {
            var arr = new[] { x ?? "", y ?? "" };
            Array.Sort(arr, StringComparer.Ordinal);
            return arr[0] + "|" + arr[1];
        }
        return Key(aHome, aAway) == Key(bHome, bAway);
    }

    private ResolvedKnockoutMatch GetKnockoutMatch(string matchId) =>
        GetKnockoutMatches().First(m => m.Fixture.MatchId == matchId);

    private KnockoutResultState GetOrCreateResult(ResolvedKnockoutMatch m)
    {
        var rs = _state.KnockoutResults.TryGetValue(m.Fixture.MatchId, out var existing)
            ? existing : new KnockoutResultState();
        // If the pairing changed since this result was stored, discard the stale values.
        if (!SamePairing(rs.HomeTeamId, rs.AwayTeamId, m.Home.Team?.Id, m.Away.Team?.Id))
        {
            rs = new KnockoutResultState();
        }
        rs.HomeTeamId = m.Home.Team?.Id;
        rs.AwayTeamId = m.Away.Team?.Id;
        return rs;
    }

    public async Task SetKnockoutWinnerAsync(string matchId, string teamId)
    {
        var m = GetKnockoutMatch(matchId);
        var rs = GetOrCreateResult(m);
        rs.WinnerTeamId = teamId;
        _state.KnockoutResults[matchId] = rs;
        await PersistAsync();
    }

    public async Task ClearKnockoutAsync(string matchId)
    {
        if (_state.KnockoutResults.Remove(matchId))
            await PersistAsync();
    }

    /// <summary>Updates a single side of a knockout score, reading the latest stored value first.</summary>
    public async Task SetKnockoutScoreSideAsync(string matchId, int? value, bool isHome)
    {
        var m = GetKnockoutMatch(matchId);
        var rs = GetOrCreateResult(m);
        if (isHome) rs.HomeScore = value; else rs.AwayScore = value;
        _state.KnockoutResults[matchId] = rs;
        await PersistAsync();
    }

    public async Task SetKnockoutPenaltiesAsync(string matchId, bool penalties)
    {
        var m = GetKnockoutMatch(matchId);
        var rs = GetOrCreateResult(m);
        rs.DecidedByPenalties = penalties;
        _state.KnockoutResults[matchId] = rs;
        await PersistAsync();
    }

    public Team? Champion =>
        GetKnockoutMatches().FirstOrDefault(m => m.Fixture.Round == KnockoutRound.Final)?.Winner;

    // ----- Reset -----------------------------------------------------------

    public async Task ResetAllAsync()
    {
        _state = new TournamentState();
        await storage.RemoveAsync(StorageKey);
        OnChange?.Invoke();
    }
}
