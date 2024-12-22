using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().Select(long.Parse).ToList();

        for (var i = 0; i < 2000; i++)
        {
            input = input.Select(Evolve).ToList();
        }

        return input.Sum().ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().Select(long.Parse).ToList();
        var vals = new Dictionary<(long, long, long, long), long>();
        var found = Enumerable.Range(0, input.Count).Select(_ => new HashSet<(long, long, long, long)>()).ToArray();

        const int iterations = 2000;
        var result = new long[iterations][];
        for (var i = 0; i < iterations; i++)
        {
            input = input.Select(Evolve).ToList();
            result[i] = input.Select(x => x % 10).ToArray();

            if (i < 4) continue;

            for (var j = 0; j < input.Count; j++)
            {
                var key = (
                    result[i - 3][j] - result[i - 4][j],
                    result[i - 2][j] - result[i - 3][j],
                    result[i - 1][j] - result[i - 2][j],
                    result[i][j] - result[i - 1][j]
                );

                if (found[j].Contains(key)) continue;
                if (!vals.TryAdd(key, result[i][j]))
                {
                    vals[key] += result[i][j];
                }
                found[j].Add(key);
            }
        }

        return vals.Values.Max().ToString();
    }

    private static long Evolve(long n)
    {
        n ^= n * 64;
        n %= 16777216;
        n ^= n / 32;
        n %= 16777216;
        n ^= n * 2048;
        n %= 16777216;
        return n;
    }
}