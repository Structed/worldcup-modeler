using WorldCupModeler.Models;

namespace WorldCupModeler.Data;

/// <summary>
/// Official, real-world FIFA World Cup 2026 results for matches that have already been
/// played. Group fixtures are keyed by their fixture id ("A-1".."L-6"); knockout matches
/// by "M73".."M104". These are applied on top of whatever the user has saved so that an
/// already-played match always reflects its real scoreline (a played result is a fact,
/// not a prediction). Every other match remains fully editable.
///
/// Group scores are entered home-team-first, matching the order each fixture is listed in
/// (see <see cref="Bracket2026.GroupFixtures"/>).
///
/// Last updated: 12 June 2026 (after the 11 June opening-day fixtures).
/// </summary>
public static class Results2026
{
    /// <summary>Final group-stage scores for matches that have been played.</summary>
    public static readonly IReadOnlyDictionary<string, MatchScore> GroupResults =
        new Dictionary<string, MatchScore>
        {
            // ----- Group A -----
            ["A-1"] = new() { HomeScore = 2, AwayScore = 0 }, // Mexico 2–0 South Africa
            ["A-2"] = new() { HomeScore = 2, AwayScore = 1 }, // South Korea 2–1 Czechia
        };

    /// <summary>Final knockout results for matches that have been played.</summary>
    public static readonly IReadOnlyDictionary<string, OfficialKnockoutResult> KnockoutResults =
        new Dictionary<string, OfficialKnockoutResult>();
}

/// <summary>
/// An official knockout result. The participating team ids are resolved from the bracket
/// at apply time, so only the winner, the scoreline and the penalties flag are specified.
/// </summary>
public record OfficialKnockoutResult(
    string WinnerTeamId,
    int? HomeScore,
    int? AwayScore,
    bool DecidedByPenalties = false);
