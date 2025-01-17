using System.Collections.Immutable;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split('/').Select(int.Parse).ToArray())
            .Select((x, i) => (x[0], x[1]))
            .ToImmutableList();

        return Search(input).ToString();
        int Search(IImmutableList<(int, int)> e, int cur = 0, int strength = 0)
        {
            return e.Where(x => x.Item1 == cur || x.Item2 == cur)
                .Select(x =>
                    Search(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2))
                .Concat([strength])
                .Max();
        }
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split('/').Select(int.Parse).ToArray())
            .Select((x, i) => (x[0], x[1]))
            .ToImmutableList();

        return Search(input).Item2.ToString();

        (int, int) Search(IImmutableList<(int, int)> e, int cur = 0, int strength = 0, int length = 0)
        {
            return e.Where(x => x.Item1 == cur || x.Item2 == cur)
                .Select(x =>
                    Search(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2, length + 1))
                .Concat([(length, strength)])
                .OrderByDescending(x => x.Item1)
                .ThenByDescending(x => x.Item2)
                .First();
        }
    }
}