using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        return this
            .InputToLines()
            .Select(x => x.Split(' '))
            .Count(x => x.Length == x.Distinct().Count())
            .ToString();
    }

    public string SolveSecond()
    {
        return this
            .InputToLines()
            .Select(x => x.Split(' ').Select(y => new string(y.Order().ToArray())).ToList())
            .Count(x => x.Count == x.Distinct().Count())
            .ToString();
    }
}