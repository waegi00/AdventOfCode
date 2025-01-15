using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        return input
            .Where((t, i) => t == input[(i + 1) % input.Length])
            .Sum(t => t - '0')
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        return input
            .Where((t, i) => t == input[(i + input.Length / 2) % input.Length])
            .Sum(t => t - '0')
            .ToString();
    }
}