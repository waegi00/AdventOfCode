using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public partial class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();
        var regex = MulRegex();

        var res = 0;
        foreach (var match in regex.Matches(input).Cast<Match>())
        {
            var i = match.Index;
            var l = match.Length;

            var sub = input[(i + 4)..(i + l - 1)];
            var splits = sub.Split(',').Select(int.Parse).ToArray();
            res += splits[0] * splits[1];
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();
        var regex = MulDoDontRegex();
        var on = true;

        var res = 0;
        foreach (var match in regex.Matches(input).Cast<Match>())
        {
            var i = match.Index;
            var l = match.Length;

            switch (match.Value)
            {
                case "do()":
                    on = true;
                    break;
                case "don't()":
                    on = false;
                    break;
                default:
                    if (on)
                    {
                        var sub = input[(i + 4)..(i + l - 1)];
                        var splits = sub.Split(',').Select(int.Parse).ToArray();
                        res += splits[0] * splits[1];
                    }
                    break;
            }
        }

        return res.ToString();
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();


    [GeneratedRegex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)")]
    private static partial Regex MulDoDontRegex();
}