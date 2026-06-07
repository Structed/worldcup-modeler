namespace WorldCupModeler.Models;

/// <summary>A group-stage fixture between two teams (structure only, no result).</summary>
public record GroupFixture(string MatchId, string Group, string HomeTeamId, string AwayTeamId);

/// <summary>A computed standings row for one team in a group.</summary>
public class Standing
{
    public required Team Team { get; init; }
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => Won * 3 + Drawn;

    /// <summary>1-based finishing position within the group.</summary>
    public int Position { get; set; }
}

/// <summary>A third-placed team being considered for one of the 8 wildcard spots.</summary>
public class ThirdPlaceEntry
{
    public required Standing Standing { get; init; }
    public required string Group { get; init; }
    public int Rank { get; set; }
    public bool Qualified { get; set; }
}
