using WorldCupModeler.Models;

namespace WorldCupModeler.Data;

/// <summary>
/// The 48 teams of the 2026 FIFA World Cup, as drawn on 5 December 2025
/// (with the six March 2026 play-off slots filled by their qualified winners).
/// Order within each group follows the official group listing; it is used only
/// as the final standings tiebreaker (seed order).
/// </summary>
public static class Teams2026
{
    public static readonly IReadOnlyList<Team> All = new List<Team>
    {
        // Group A
        new("MEX", "Mexico", "mx", "A"),
        new("RSA", "South Africa", "za", "A"),
        new("KOR", "South Korea", "kr", "A"),
        new("CZE", "Czechia", "cz", "A"),

        // Group B
        new("CAN", "Canada", "ca", "B"),
        new("BIH", "Bosnia and Herzegovina", "ba", "B"),
        new("QAT", "Qatar", "qa", "B"),
        new("SUI", "Switzerland", "ch", "B"),

        // Group C
        new("BRA", "Brazil", "br", "C"),
        new("MAR", "Morocco", "ma", "C"),
        new("HAI", "Haiti", "ht", "C"),
        new("SCO", "Scotland", "gb-sct", "C"),

        // Group D
        new("USA", "United States", "us", "D"),
        new("PAR", "Paraguay", "py", "D"),
        new("AUS", "Australia", "au", "D"),
        new("TUR", "Türkiye", "tr", "D"),

        // Group E
        new("GER", "Germany", "de", "E"),
        new("CUW", "Curaçao", "cw", "E"),
        new("CIV", "Ivory Coast", "ci", "E"),
        new("ECU", "Ecuador", "ec", "E"),

        // Group F
        new("NED", "Netherlands", "nl", "F"),
        new("JPN", "Japan", "jp", "F"),
        new("SWE", "Sweden", "se", "F"),
        new("TUN", "Tunisia", "tn", "F"),

        // Group G
        new("BEL", "Belgium", "be", "G"),
        new("EGY", "Egypt", "eg", "G"),
        new("IRN", "Iran", "ir", "G"),
        new("NZL", "New Zealand", "nz", "G"),

        // Group H
        new("ESP", "Spain", "es", "H"),
        new("CPV", "Cape Verde", "cv", "H"),
        new("KSA", "Saudi Arabia", "sa", "H"),
        new("URU", "Uruguay", "uy", "H"),

        // Group I
        new("FRA", "France", "fr", "I"),
        new("SEN", "Senegal", "sn", "I"),
        new("IRQ", "Iraq", "iq", "I"),
        new("NOR", "Norway", "no", "I"),

        // Group J
        new("ARG", "Argentina", "ar", "J"),
        new("ALG", "Algeria", "dz", "J"),
        new("AUT", "Austria", "at", "J"),
        new("JOR", "Jordan", "jo", "J"),

        // Group K
        new("POR", "Portugal", "pt", "K"),
        new("COD", "DR Congo", "cd", "K"),
        new("UZB", "Uzbekistan", "uz", "K"),
        new("COL", "Colombia", "co", "K"),

        // Group L
        new("ENG", "England", "gb-eng", "L"),
        new("CRO", "Croatia", "hr", "L"),
        new("GHA", "Ghana", "gh", "L"),
        new("PAN", "Panama", "pa", "L"),
    };

    public static readonly IReadOnlyList<string> Groups =
        All.Select(t => t.Group).Distinct().OrderBy(g => g).ToList();

    public static readonly IReadOnlyDictionary<string, Team> ById =
        All.ToDictionary(t => t.Id);

    public static IEnumerable<Team> InGroup(string group) =>
        All.Where(t => t.Group == group);
}
