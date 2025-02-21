using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2020.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var jolts = this.InputToLines()
            .Select(long.Parse)
            .Order()
            .ToArray();

        var differences = new Dictionary<long, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 1 },
        };
        differences[jolts[0]]++;

        for (var i = 0; i < jolts.Length - 1; i++)
        {
            differences[jolts[i + 1] - jolts[i]]++;
        }

        return (differences[1] * differences[3]).ToString();
    }

    public string SolveSecond()
    {
        var jolts = this.InputToLines()
            .Select(long.Parse)
            .Order()
            .ToList();

        jolts.Insert(0, 0);
        jolts.Add(jolts.Last() + 3);

        var joltGraph = jolts
            .ToDictionary(
                x => x, 
                x => jolts.Where(y => y.IsBetween(x + 1, x + 3)).ToHashSet());

        var cache = new Dictionary<long, long>();

        return DepthFirstCounter(joltGraph, 0).ToString();

        long DepthFirstCounter(Dictionary<long, HashSet<long>> graph, long pos)
        {
            if (cache.TryGetValue(pos, out var dfc)) return dfc;

            if (graph[pos].Count <= 0) return 1;
            var result = graph[pos].Sum(x => DepthFirstCounter(graph, x));
            cache[pos] = result;
            return result;

        }
    }
}