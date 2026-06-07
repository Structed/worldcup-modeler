namespace WorldCupModeler.Models;

/// <summary>The persisted, user-entered state. Saved to localStorage as one JSON blob.</summary>
public class TournamentState
{
    public int Version { get; set; } = 1;

    /// <summary>Group match results keyed by MatchId.</summary>
    public Dictionary<string, MatchScore> GroupResults { get; set; } = new();

    /// <summary>Optional manual ordering of teams per group (groupId -> ordered teamIds).</summary>
    public Dictionary<string, List<string>> GroupOrderOverrides { get; set; } = new();

    /// <summary>Knockout results keyed by MatchId.</summary>
    public Dictionary<string, KnockoutResultState> KnockoutResults { get; set; } = new();
}

public class MatchScore
{
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
}

public class KnockoutResultState
{
    public string? WinnerTeamId { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public bool DecidedByPenalties { get; set; }

    /// <summary>The pairing (resolved team ids) this result was entered for; used to
    /// invalidate the result if an upstream change alters either participant.</summary>
    public string? HomeTeamId { get; set; }
    public string? AwayTeamId { get; set; }
}
