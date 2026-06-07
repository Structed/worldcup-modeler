namespace WorldCupModeler.Models;

public enum SlotSourceKind
{
    /// <summary>A finishing position within a group, e.g. winner (1) or runner-up (2).</summary>
    GroupPosition,
    /// <summary>One of the eight best third-placed teams, anchored to a winner's group.</summary>
    ThirdPlace,
    /// <summary>The winner of an earlier knockout match.</summary>
    MatchWinner,
    /// <summary>The loser of an earlier knockout match (third-place playoff).</summary>
    MatchLoser
}

/// <summary>Describes where a knockout slot's team comes from.</summary>
public record SlotSource(SlotSourceKind Kind, string? Group = null, int Position = 0, string? MatchId = null)
{
    public static SlotSource GroupPos(string group, int position) =>
        new(SlotSourceKind.GroupPosition, Group: group, Position: position);

    /// <summary>A third-place slot, anchored to the group of the group-winner it faces.</summary>
    public static SlotSource Third(string winnerGroup) =>
        new(SlotSourceKind.ThirdPlace, Group: winnerGroup);

    public static SlotSource Winner(string matchId) =>
        new(SlotSourceKind.MatchWinner, MatchId: matchId);

    public static SlotSource Loser(string matchId) =>
        new(SlotSourceKind.MatchLoser, MatchId: matchId);
}

public enum KnockoutRound
{
    RoundOf32,
    RoundOf16,
    QuarterFinal,
    SemiFinal,
    ThirdPlacePlayoff,
    Final
}

/// <summary>Structural definition of a knockout match (no result).</summary>
public record KnockoutFixture(
    string MatchId,
    int MatchNumber,
    KnockoutRound Round,
    SlotSource Home,
    SlotSource Away);

/// <summary>A team slot resolved to an actual team (or null when not yet determined).</summary>
public record ResolvedSlot(Team? Team, string Label);

/// <summary>A knockout match with both slots resolved and the current result applied.</summary>
public class ResolvedKnockoutMatch
{
    public required KnockoutFixture Fixture { get; init; }
    public required ResolvedSlot Home { get; init; }
    public required ResolvedSlot Away { get; init; }
    public string? WinnerTeamId { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public bool DecidedByPenalties { get; set; }

    public bool BothTeamsKnown => Home.Team is not null && Away.Team is not null;
    public Team? Winner => WinnerTeamId is null ? null
        : (Home.Team?.Id == WinnerTeamId ? Home.Team
        : Away.Team?.Id == WinnerTeamId ? Away.Team : null);
    public Team? Loser => Winner is null ? null
        : (Winner.Id == Home.Team?.Id ? Away.Team : Home.Team);
}
