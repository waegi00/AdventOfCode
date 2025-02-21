using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day09 : IRiddle
{
    private const int preambleLength = 25;

    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(long.Parse)
            .ToArray();

        var set = new HashSet<long>(input[..preambleLength]);

        for (var i = preambleLength; i < input.Length; i++)
        {
            if (!set.Any(x => set.Contains(input[i] - x)))
            {
                return input[i].ToString();
            }

            set.Remove(input[i - preambleLength]);
            set.Add(input[i]);
        }

        return "";
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(long.Parse)
            .ToArray();

        var invalidNumber = int.Parse(SolveFirst());

        for (var i = 0; i < input.Length; i++)
        {
            var total = 0L;

            for (var j = i; j < input.Length && total < invalidNumber; j++)
            {
                total += input[j];
                if (j <= i || total != invalidNumber) continue;
                var result = input[i..(j + 1)];
                return (result.Min() + result.Max()).ToString();
            }
        }

        return "";
    }
}