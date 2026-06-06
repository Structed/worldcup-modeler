using WorldCupModeler.Models;

namespace WorldCupModeler.Data;

/// <summary>
/// Static structure of the 2026 tournament: group round-robin fixtures and the official
/// knockout bracket (matches 73-104), based on the published FIFA schedule.
/// </summary>
public static class Bracket2026
{
    /// <summary>Six round-robin fixtures per group, generated from the seeded team order.</summary>
    public static IReadOnlyList<GroupFixture> GroupFixtures(string group)
    {
        var t = Teams2026.InGroup(group).ToList();
        // Standard 4-team round robin pairing schedule.
        var pairs = new (int h, int a)[] { (0, 1), (2, 3), (0, 2), (3, 1), (0, 3), (1, 2) };
        var result = new List<GroupFixture>();
        for (var i = 0; i < pairs.Length; i++)
        {
            var (h, a) = pairs[i];
            result.Add(new GroupFixture($"{group}-{i + 1}", group, t[h].Id, t[a].Id));
        }
        return result;
    }

    public static IReadOnlyList<GroupFixture> AllGroupFixtures =>
        Teams2026.Groups.SelectMany(GroupFixtures).ToList();

    /// <summary>
    /// The eight Round-of-32 group winners (in match order) that face a best third-placed team.
    /// Used both to build R32 fixtures and to drive the third-place slot assignment.
    /// </summary>
    public static readonly IReadOnlyList<string> ThirdPlaceWinnerGroups =
        new[] { "E", "I", "A", "L", "D", "G", "B", "K" };

    public static readonly IReadOnlyList<KnockoutFixture> KnockoutFixtures = BuildKnockout();

    public static IReadOnlyDictionary<string, KnockoutFixture> KnockoutById { get; } =
        KnockoutFixtures.ToDictionary(f => f.MatchId);

    private static List<KnockoutFixture> BuildKnockout()
    {
        SlotSource G(string group, int pos) => SlotSource.GroupPos(group, pos);
        SlotSource T(string anchor) => SlotSource.Third(anchor);
        SlotSource W(int n) => SlotSource.Winner($"M{n}");
        SlotSource L(int n) => SlotSource.Loser($"M{n}");
        KnockoutFixture F(int n, KnockoutRound r, SlotSource h, SlotSource a) =>
            new($"M{n}", n, r, h, a);

        var r32 = KnockoutRound.RoundOf32;
        return new List<KnockoutFixture>
        {
            // Round of 32 (73-88)
            F(73, r32, G("A", 2), G("B", 2)),
            F(74, r32, G("E", 1), T("E")),
            F(75, r32, G("F", 1), G("C", 2)),
            F(76, r32, G("C", 1), G("F", 2)),
            F(77, r32, G("I", 1), T("I")),
            F(78, r32, G("E", 2), G("I", 2)),
            F(79, r32, G("A", 1), T("A")),
            F(80, r32, G("L", 1), T("L")),
            F(81, r32, G("D", 1), T("D")),
            F(82, r32, G("G", 1), T("G")),
            F(83, r32, G("K", 2), G("L", 2)),
            F(84, r32, G("H", 1), G("J", 2)),
            F(85, r32, G("B", 1), T("B")),
            F(86, r32, G("J", 1), G("H", 2)),
            F(87, r32, G("K", 1), T("K")),
            F(88, r32, G("D", 2), G("G", 2)),

            // Round of 16 (89-96)
            F(89, KnockoutRound.RoundOf16, W(74), W(77)),
            F(90, KnockoutRound.RoundOf16, W(73), W(75)),
            F(91, KnockoutRound.RoundOf16, W(76), W(79)),
            F(92, KnockoutRound.RoundOf16, W(78), W(80)),
            F(93, KnockoutRound.RoundOf16, W(81), W(84)),
            F(94, KnockoutRound.RoundOf16, W(82), W(85)),
            F(95, KnockoutRound.RoundOf16, W(83), W(86)),
            F(96, KnockoutRound.RoundOf16, W(87), W(88)),

            // Quarter-finals (97-100)
            F(97, KnockoutRound.QuarterFinal, W(89), W(90)),
            F(98, KnockoutRound.QuarterFinal, W(91), W(92)),
            F(99, KnockoutRound.QuarterFinal, W(93), W(94)),
            F(100, KnockoutRound.QuarterFinal, W(95), W(96)),

            // Semi-finals (101-102)
            F(101, KnockoutRound.SemiFinal, W(97), W(98)),
            F(102, KnockoutRound.SemiFinal, W(99), W(100)),

            // Third-place play-off (103) and Final (104)
            F(103, KnockoutRound.ThirdPlacePlayoff, L(101), L(102)),
            F(104, KnockoutRound.Final, W(101), W(102)),
        };
    }
}
