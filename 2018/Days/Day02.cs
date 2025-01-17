using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var twos = 0;
        var threes = 0;

        foreach (var line in input)
        {
            var f = line
                .GroupBy(x => x)
                .Select(x => x.Count())
                .ToArray();

            if (f.Any(x => x == 2))
            {
                twos++;
            }

            if (f.Any(x => x == 3))
            {
                threes++;
            }
        }

        return (twos * threes).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        for (var i = 0; i < input.Length; i++)
        {
            var pair = input
                .Select(x => x.Remove(i, 1))
                .GroupBy(x => x)
                .FirstOrDefault(x => x.Count() > 1);

            if (pair != null)
            {
                return pair.First();
            }
        }

        return "";
    }
}