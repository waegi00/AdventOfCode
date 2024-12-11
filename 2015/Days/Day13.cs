using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var dict = input.Select(line => line.Split(' '))
            .ToDictionary(splits => (splits[0], splits[^1][..^1]), splits => (splits[2] == "lose" ? -1 : 1) * int.Parse(splits[3]));

        var people = dict.Keys.SelectMany(t => new[] { t.Item1, t.Item2 }).Distinct().ToList();

        var sum = people.GetPermutations(people.Count)
            .Select(p => p.ToArray())
            .Select(perm => perm.Select((t, i) => dict[(t, perm[(i + 1) % perm.Length])] + dict[(perm[(i + 1) % perm.Length], t)]).Sum())
            .Prepend(0)
            .Max();

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var dict = input.Select(line => line.Split(' '))
            .ToDictionary(splits => (splits[0], splits[^1][..^1]), splits => (splits[2] == "lose" ? -1 : 1) * int.Parse(splits[3]));

        var people = dict.Keys.SelectMany(t => new[] { t.Item1, t.Item2 }).Distinct().ToList();

        foreach (var p in people)
        {
            dict[("me", p)] = dict[(p, "me")] = 0;
        }

        people.Add("me");

        var sum = people.GetPermutations(people.Count)
            .Select(p => p.ToArray())
            .Select(perm => perm.Select((t, i) => dict[(t, perm[(i + 1) % perm.Length])] + dict[(perm[(i + 1) % perm.Length], t)]).Sum())
            .Prepend(0)
            .Max();

        return sum.ToString();
    }
}