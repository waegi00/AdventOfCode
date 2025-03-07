using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        return LanternFish(
            80,
            this.InputToText()
                .Split(',')
                .Select(long.Parse)
                .ToArray())
            .ToString();
    }

    public string SolveSecond()
    {
        return LanternFish(
            256,
            this.InputToText()
                .Split(',')
                .Select(long.Parse)
                .ToArray())
            .ToString();
    }

    private static long LanternFish(long days, long[] start)
    {
        var lanternFish = new long[9];

        foreach (var s in start)
        {
            lanternFish[s]++;
        }

        for (var i = 0; i < days; i++)
        {
            var zero = lanternFish[0];

            for (var j = 1; j < lanternFish.Length; j++)
            {
                lanternFish[j - 1] = lanternFish[j];
                lanternFish[j] = 0;
            }

            lanternFish[6] += zero;
            lanternFish[8] = zero;
        }

        return lanternFish.Sum();
    }
}