namespace WorldCupModeler.Models;

/// <summary>
/// Scheduling info for a single match: the absolute kickoff instant (UTC) plus the
/// venue. <see cref="KickoffUtc"/> is stored in UTC so the UI can render it in the
/// user's own timezone and locale.
/// </summary>
public record MatchInfo(DateTimeOffset KickoffUtc, string Stadium, string City);
