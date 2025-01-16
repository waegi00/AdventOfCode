using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var generators = this.InputToLines()
            .Select(x => long.Parse(x.Split(' ')[^1]))
            .ToArray();

        var factors = new[] { 16807L, 48271L };
        const int mod = 2147483647;

        var matches = 0;
        for (var i = 0; i < 40_000_000; i++)
        {
            for (var g = 0; g < generators.Length; g++)
            {
                generators[g] = generators[g] * factors[g] % mod;
            }

            if ((generators[0] & 0xFFFF) == (generators[1] & 0xFFFF))
            {
                matches++;
            }
        }

        return matches.ToString();
    }

    public string SolveSecond()
    {
        var generators = this.InputToLines()
            .Select(x => long.Parse(x.Split(' ')[^1]))
            .ToArray();

        var factors = new[] { 16807L, 48271L };
        const int mod = 2147483647;

        const int length = 5_000_000;
        var pairs = new (long a, long b)[length];
        var a = 0;
        var b = 0;

        while (a < length || b < length)
        {
            if (a < length)
            {
                generators[0] = generators[0] * factors[0] % mod;

                if ((generators[0] & 0b11) == 0)
                {
                    pairs[a].a = generators[0] & 0xFFFF;
                    a++;
                }
            }

            if (b >= length) continue;

            generators[1] = generators[1] * factors[1] % mod;

            if ((generators[1] & 0b111) != 0) continue;

            pairs[b].b = generators[1] & 0xFFFF;
            b++;
        }

        return pairs.Count(x => x.a == x.b).ToString();
    }
}