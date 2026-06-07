using WorldCupModeler.Models;

namespace WorldCupModeler.Data;

/// <summary>
/// Official 2026 FIFA World Cup match schedule: kickoff instant (UTC) and venue for
/// each match. Group fixtures are keyed by their fixture id ("A-1".."L-6"); knockout
/// matches by "M73".."M104".
///
/// Source times are published in US Eastern Time; the entire tournament window
/// (11 Jun - 19 Jul 2026) is on Eastern Daylight Time (UTC-4), so each UTC instant is
/// the ET kickoff plus four hours.
/// </summary>
public static class Schedule2026
{
    /// <summary>Build a UTC kickoff instant from the published ET (EDT, UTC-4) kickoff.</summary>
    private static DateTimeOffset Et(int year, int month, int day, int hour, int minute) =>
        new DateTimeOffset(year, month, day, hour, minute, 0, TimeSpan.FromHours(-4)).ToUniversalTime();

    private static readonly IReadOnlyDictionary<string, MatchInfo> Data =
        new Dictionary<string, MatchInfo>
        {
            // ----- Group A -----
            ["A-1"] = new(Et(2026, 6, 11, 15, 0), "Estadio Azteca", "Mexico City"),
            ["A-2"] = new(Et(2026, 6, 11, 22, 0), "Estadio Akron", "Guadalajara"),
            ["A-3"] = new(Et(2026, 6, 18, 21, 0), "Estadio Akron", "Guadalajara"),
            ["A-4"] = new(Et(2026, 6, 18, 12, 0), "Mercedes-Benz Stadium", "Atlanta"),
            ["A-5"] = new(Et(2026, 6, 24, 21, 0), "Estadio Azteca", "Mexico City"),
            ["A-6"] = new(Et(2026, 6, 24, 21, 0), "Estadio BBVA", "Monterrey"),

            // ----- Group B -----
            ["B-1"] = new(Et(2026, 6, 12, 15, 0), "BMO Field", "Toronto"),
            ["B-2"] = new(Et(2026, 6, 13, 15, 0), "Levi's Stadium", "San Francisco Bay Area"),
            ["B-3"] = new(Et(2026, 6, 18, 18, 0), "BC Place", "Vancouver"),
            ["B-4"] = new(Et(2026, 6, 18, 15, 0), "SoFi Stadium", "Los Angeles"),
            ["B-5"] = new(Et(2026, 6, 24, 15, 0), "BC Place", "Vancouver"),
            ["B-6"] = new(Et(2026, 6, 24, 15, 0), "Lumen Field", "Seattle"),

            // ----- Group C -----
            ["C-1"] = new(Et(2026, 6, 13, 18, 0), "MetLife Stadium", "East Rutherford"),
            ["C-2"] = new(Et(2026, 6, 13, 21, 0), "Gillette Stadium", "Boston"),
            ["C-3"] = new(Et(2026, 6, 19, 20, 30), "Lincoln Financial Field", "Philadelphia"),
            ["C-4"] = new(Et(2026, 6, 19, 18, 0), "Gillette Stadium", "Boston"),
            ["C-5"] = new(Et(2026, 6, 24, 18, 0), "Hard Rock Stadium", "Miami Gardens"),
            ["C-6"] = new(Et(2026, 6, 24, 18, 0), "Mercedes-Benz Stadium", "Atlanta"),

            // ----- Group D -----
            ["D-1"] = new(Et(2026, 6, 12, 21, 0), "SoFi Stadium", "Los Angeles"),
            ["D-2"] = new(Et(2026, 6, 14, 0, 0), "BC Place", "Vancouver"),
            ["D-3"] = new(Et(2026, 6, 19, 15, 0), "Lumen Field", "Seattle"),
            ["D-4"] = new(Et(2026, 6, 19, 23, 0), "Levi's Stadium", "San Francisco Bay Area"),
            ["D-5"] = new(Et(2026, 6, 25, 22, 0), "SoFi Stadium", "Los Angeles"),
            ["D-6"] = new(Et(2026, 6, 25, 22, 0), "Levi's Stadium", "San Francisco Bay Area"),

            // ----- Group E -----
            ["E-1"] = new(Et(2026, 6, 14, 13, 0), "NRG Stadium", "Houston"),
            ["E-2"] = new(Et(2026, 6, 14, 19, 0), "Lincoln Financial Field", "Philadelphia"),
            ["E-3"] = new(Et(2026, 6, 20, 16, 0), "BMO Field", "Toronto"),
            ["E-4"] = new(Et(2026, 6, 20, 20, 0), "Arrowhead Stadium", "Kansas City"),
            ["E-5"] = new(Et(2026, 6, 25, 16, 0), "MetLife Stadium", "East Rutherford"),
            ["E-6"] = new(Et(2026, 6, 25, 16, 0), "Lincoln Financial Field", "Philadelphia"),

            // ----- Group F -----
            ["F-1"] = new(Et(2026, 6, 14, 16, 0), "AT&T Stadium", "Dallas"),
            ["F-2"] = new(Et(2026, 6, 14, 22, 0), "Estadio BBVA", "Monterrey"),
            ["F-3"] = new(Et(2026, 6, 20, 13, 0), "NRG Stadium", "Houston"),
            ["F-4"] = new(Et(2026, 6, 21, 0, 0), "Estadio BBVA", "Monterrey"),
            ["F-5"] = new(Et(2026, 6, 25, 19, 0), "Arrowhead Stadium", "Kansas City"),
            ["F-6"] = new(Et(2026, 6, 25, 19, 0), "AT&T Stadium", "Dallas"),

            // ----- Group G -----
            ["G-1"] = new(Et(2026, 6, 15, 15, 0), "Lumen Field", "Seattle"),
            ["G-2"] = new(Et(2026, 6, 15, 21, 0), "SoFi Stadium", "Los Angeles"),
            ["G-3"] = new(Et(2026, 6, 21, 15, 0), "SoFi Stadium", "Los Angeles"),
            ["G-4"] = new(Et(2026, 6, 21, 21, 0), "BC Place", "Vancouver"),
            ["G-5"] = new(Et(2026, 6, 26, 23, 0), "BC Place", "Vancouver"),
            ["G-6"] = new(Et(2026, 6, 26, 23, 0), "Lumen Field", "Seattle"),

            // ----- Group H -----
            ["H-1"] = new(Et(2026, 6, 15, 12, 0), "Mercedes-Benz Stadium", "Atlanta"),
            ["H-2"] = new(Et(2026, 6, 15, 18, 0), "Hard Rock Stadium", "Miami Gardens"),
            ["H-3"] = new(Et(2026, 6, 21, 12, 0), "Mercedes-Benz Stadium", "Atlanta"),
            ["H-4"] = new(Et(2026, 6, 21, 18, 0), "Hard Rock Stadium", "Miami Gardens"),
            ["H-5"] = new(Et(2026, 6, 26, 20, 0), "Estadio Akron", "Guadalajara"),
            ["H-6"] = new(Et(2026, 6, 26, 20, 0), "NRG Stadium", "Houston"),

            // ----- Group I -----
            ["I-1"] = new(Et(2026, 6, 16, 15, 0), "MetLife Stadium", "East Rutherford"),
            ["I-2"] = new(Et(2026, 6, 16, 18, 0), "Gillette Stadium", "Boston"),
            ["I-3"] = new(Et(2026, 6, 22, 17, 0), "Lincoln Financial Field", "Philadelphia"),
            ["I-4"] = new(Et(2026, 6, 22, 20, 0), "MetLife Stadium", "East Rutherford"),
            ["I-5"] = new(Et(2026, 6, 26, 15, 0), "Gillette Stadium", "Boston"),
            ["I-6"] = new(Et(2026, 6, 26, 15, 0), "BMO Field", "Toronto"),

            // ----- Group J -----
            ["J-1"] = new(Et(2026, 6, 16, 21, 0), "Arrowhead Stadium", "Kansas City"),
            ["J-2"] = new(Et(2026, 6, 17, 0, 0), "Levi's Stadium", "San Francisco Bay Area"),
            ["J-3"] = new(Et(2026, 6, 22, 13, 0), "AT&T Stadium", "Dallas"),
            ["J-4"] = new(Et(2026, 6, 22, 23, 0), "Levi's Stadium", "San Francisco Bay Area"),
            ["J-5"] = new(Et(2026, 6, 27, 22, 0), "AT&T Stadium", "Dallas"),
            ["J-6"] = new(Et(2026, 6, 27, 22, 0), "Arrowhead Stadium", "Kansas City"),

            // ----- Group K -----
            ["K-1"] = new(Et(2026, 6, 17, 13, 0), "NRG Stadium", "Houston"),
            ["K-2"] = new(Et(2026, 6, 17, 22, 0), "Estadio Azteca", "Mexico City"),
            ["K-3"] = new(Et(2026, 6, 23, 13, 0), "NRG Stadium", "Houston"),
            ["K-4"] = new(Et(2026, 6, 23, 22, 0), "Estadio Akron", "Guadalajara"),
            ["K-5"] = new(Et(2026, 6, 27, 19, 30), "Hard Rock Stadium", "Miami Gardens"),
            ["K-6"] = new(Et(2026, 6, 27, 19, 30), "Mercedes-Benz Stadium", "Atlanta"),

            // ----- Group L -----
            ["L-1"] = new(Et(2026, 6, 17, 16, 0), "AT&T Stadium", "Dallas"),
            ["L-2"] = new(Et(2026, 6, 17, 19, 0), "BMO Field", "Toronto"),
            ["L-3"] = new(Et(2026, 6, 23, 16, 0), "Gillette Stadium", "Boston"),
            ["L-4"] = new(Et(2026, 6, 23, 19, 0), "BMO Field", "Toronto"),
            ["L-5"] = new(Et(2026, 6, 27, 17, 0), "MetLife Stadium", "East Rutherford"),
            ["L-6"] = new(Et(2026, 6, 27, 17, 0), "Lincoln Financial Field", "Philadelphia"),

            // ----- Round of 32 -----
            ["M73"] = new(Et(2026, 6, 28, 15, 0), "SoFi Stadium", "Los Angeles"),
            ["M74"] = new(Et(2026, 6, 29, 16, 30), "Gillette Stadium", "Boston"),
            ["M75"] = new(Et(2026, 6, 29, 21, 0), "Estadio BBVA", "Monterrey"),
            ["M76"] = new(Et(2026, 6, 29, 13, 0), "NRG Stadium", "Houston"),
            ["M77"] = new(Et(2026, 6, 30, 17, 0), "MetLife Stadium", "East Rutherford"),
            ["M78"] = new(Et(2026, 6, 30, 13, 0), "AT&T Stadium", "Dallas"),
            ["M79"] = new(Et(2026, 6, 30, 21, 0), "Estadio Azteca", "Mexico City"),
            ["M80"] = new(Et(2026, 7, 1, 12, 0), "Mercedes-Benz Stadium", "Atlanta"),
            ["M81"] = new(Et(2026, 7, 1, 20, 0), "Levi's Stadium", "San Francisco Bay Area"),
            ["M82"] = new(Et(2026, 7, 1, 16, 0), "Lumen Field", "Seattle"),
            ["M83"] = new(Et(2026, 7, 2, 19, 0), "BMO Field", "Toronto"),
            ["M84"] = new(Et(2026, 7, 2, 15, 0), "SoFi Stadium", "Los Angeles"),
            ["M85"] = new(Et(2026, 7, 2, 23, 0), "BC Place", "Vancouver"),
            ["M86"] = new(Et(2026, 7, 3, 18, 0), "Hard Rock Stadium", "Miami Gardens"),
            ["M87"] = new(Et(2026, 7, 3, 21, 30), "Arrowhead Stadium", "Kansas City"),
            ["M88"] = new(Et(2026, 7, 3, 14, 0), "AT&T Stadium", "Dallas"),

            // ----- Round of 16 -----
            ["M89"] = new(Et(2026, 7, 4, 17, 0), "Lincoln Financial Field", "Philadelphia"),
            ["M90"] = new(Et(2026, 7, 4, 13, 0), "NRG Stadium", "Houston"),
            ["M91"] = new(Et(2026, 7, 5, 16, 0), "MetLife Stadium", "East Rutherford"),
            ["M92"] = new(Et(2026, 7, 5, 20, 0), "Estadio Azteca", "Mexico City"),
            ["M93"] = new(Et(2026, 7, 6, 15, 0), "AT&T Stadium", "Dallas"),
            ["M94"] = new(Et(2026, 7, 6, 20, 0), "Lumen Field", "Seattle"),
            ["M95"] = new(Et(2026, 7, 7, 12, 0), "Mercedes-Benz Stadium", "Atlanta"),
            ["M96"] = new(Et(2026, 7, 7, 16, 0), "BC Place", "Vancouver"),

            // ----- Quarter-finals -----
            ["M97"] = new(Et(2026, 7, 9, 16, 0), "Gillette Stadium", "Boston"),
            ["M98"] = new(Et(2026, 7, 10, 15, 0), "SoFi Stadium", "Los Angeles"),
            ["M99"] = new(Et(2026, 7, 11, 17, 0), "Hard Rock Stadium", "Miami Gardens"),
            ["M100"] = new(Et(2026, 7, 11, 21, 0), "Arrowhead Stadium", "Kansas City"),

            // ----- Semi-finals -----
            ["M101"] = new(Et(2026, 7, 14, 15, 0), "AT&T Stadium", "Dallas"),
            ["M102"] = new(Et(2026, 7, 15, 15, 0), "Mercedes-Benz Stadium", "Atlanta"),

            // ----- Third-place play-off -----
            ["M103"] = new(Et(2026, 7, 18, 17, 0), "Hard Rock Stadium", "Miami Gardens"),

            // ----- Final -----
            ["M104"] = new(Et(2026, 7, 19, 15, 0), "MetLife Stadium", "East Rutherford"),
        };

    public static MatchInfo? Get(string matchId) =>
        Data.TryGetValue(matchId, out var info) ? info : null;
}
