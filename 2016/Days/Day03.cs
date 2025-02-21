using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public partial class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var regex = MyRegex();
        var input = this.InputToLines()
            .Select(x => regex.Matches(x).Select(m => int.Parse(m.Value)).ToList())
            .ToList();

        return input.Count(x =>
            x[0] + x[1] > x[2] &&
            x[0] + x[2] > x[1] &&
            x[1] + x[2] > x[0]).ToString();
    }

    public string SolveSecond()
    {
        var regex = MyRegex();
        var input = this.InputToLines()
            .Select(x => regex.Matches(x).Select(m => int.Parse(m.Value)).ToList())
            .ToList();

        var sum = 0;

        for (var i = 0; i < input.Count; i += 3)
        {
            for (var j = 0; j < 3; j++)
            {
                if (input[i][j] + input[i + 1][j] > input[i + 2][j] &&
                    input[i][j] + input[i + 2][j] > input[i + 1][j] &&
                    input[i + 1][j] + input[i + 2][j] > input[i][j])
                {
                    sum++;
                }
            }
        }

        return sum.ToString();
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex MyRegex();
}