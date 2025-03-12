using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        return PolymerCount(10).ToString();
    }

    public string SolveSecond()
    {
        return PolymerCount(40).ToString();
    }

    private long PolymerCount(int steps)
    {
        var lines = this.InputToText()
            .Split("\r\n\r\n");

        var template = lines[0];
        var rules = lines[1]
            .Split("\r\n")
            .Select(line => line.Split(" -> "))
            .ToDictionary(parts => parts[0], parts => parts[1]);

        var pairs = new Dictionary<string, long>();
        var chars = new Dictionary<char, long>();

        for (var i = 0; i < template.Length - 1; i++)
        {
            var pair = template.Substring(i, 2);
            pairs.TryAdd(pair, 0);
            pairs[pair]++;
        }

        foreach (var c in template)
        {
            chars.TryAdd(c, 0);
            chars[c]++;
        }

        for (var step = 0; step < steps; step++)
        {
            var newPairs = new Dictionary<string, long>(pairs);
            foreach (var pair in pairs.Keys.ToList())
            {
                if (!rules.TryGetValue(pair, out var rule)) continue;
                var insert = rule[0];
                var count = pairs[pair];

                newPairs.TryAdd(pair, 0);
                newPairs[pair] -= count;

                var newPair1 = pair[0] + insert.ToString();
                var newPair2 = insert.ToString() + pair[1];

                newPairs.TryAdd(newPair1, 0);
                newPairs.TryAdd(newPair2, 0);

                newPairs[newPair1] += count;
                newPairs[newPair2] += count;

                chars.TryAdd(insert, 0);
                chars[insert] += count;
            }
            pairs = newPairs;
        }

        return chars.Values.Max() - chars.Values.Min();
    }
}