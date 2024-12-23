using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        return Connections().Count(c => c.Any(x => x.StartsWith('t'))).ToString();
    }

    public string SolveSecond()
    {
        return string.Join(',', Connections().GroupBy(x => x[0]).MaxBy(x => x.Count())!.SelectMany(x => x).Distinct().Order());
    }

    private List<List<string>> Connections()
    {
        var network = this.InputToLines()
            .Select(x => x.Split('-'))
            .SelectMany(x => new[] { new[] { x[0], x[1] }, [x[1], x[0]] })
            .GroupBy(x => x[0], x => x[1])
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.ToHashSet());

        var connections = network.Keys
            .SelectMany(c1 =>
                network[c1].SelectMany(c2 =>
                    network[c1]
                        .Intersect(network[c2])
                        .Select(c3 => string.Join(',', new[] { c1, c2, c3 }.Order()))))
            .Distinct()
            .Select(x => x.Split(',').ToList())
            .ToList();

        return connections;
    }
}