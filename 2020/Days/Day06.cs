using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToText()
            .Split("\r\n\r\n")
            .Sum(x => x.Where(char.IsLower).Distinct().Count())
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .Sum(group => group
                    .Select(row => row.ToHashSet())
                    .Aggregate((set1, set2) => set1.Intersect(set2).ToHashSet())
                    .Count)
            .ToString();
    }
}