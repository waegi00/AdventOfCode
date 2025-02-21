using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public partial class Day08 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines()
            .Sum(x => 2 + x.Length - MyRegex().Replace(x, " ").Length)
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Sum(x => 2 + Regex.Replace(x, @"\\|""", "  ").Length - x.Length)
            .ToString();
    }

    [GeneratedRegex(@"\\\\|\\\""|\\x[0-9A-Fa-f]{2}")]
    private static partial Regex MyRegex();
}