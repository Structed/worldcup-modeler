namespace WorldCupModeler.Models;

/// <summary>A national team competing in the tournament. <paramref name="Code"/> is the
/// ISO 3166 code used by the flag-icons library (e.g. "mx", or "gb-eng" for England).</summary>
public record Team(string Id, string Name, string Code, string Group);
