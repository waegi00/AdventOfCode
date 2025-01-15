using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split('\t')
            .Select(int.Parse)
            .ToArray();

        var seen = new HashSet<string>();
        var sum = 0;

        while (!seen.Contains(string.Join(" ", input)))
        {
            seen.Add(string.Join(" ", input));
            var index = input.MaxIndex();
            var n = input[index];
            input[index] = 0;
            for (var i = 0; i < n; i++)
            {
                input[++index % input.Length]++;
            }

            sum++;
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split('\t')
            .Select(int.Parse)
            .ToArray();

        var seen = new Dictionary<string, int>();
        var sum = 0;

        while (!seen.ContainsKey(string.Join(" ", input)))
        {
            seen.Add(string.Join(" ", input), sum);
            var index = input.MaxIndex();
            var n = input[index];
            input[index] = 0;
            for (var i = 0; i < n; i++)
            {
                input[++index % input.Length]++;
            }

            sum++;
        }

        return (sum - seen[string.Join(" ", input)]).ToString();
    }
}