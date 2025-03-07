using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day08 : IRiddle
{
    private static readonly char[] part1Matches = ['1', '4', '7', '8'];

    public string SolveFirst()
    {
        return this.InputToLines()
            .Select(line => line.Split('|'))
            .Sum(parts => 
                string.Join("", parts[1]
                    .Trim()
                    .Split(' ')
                    .Select(r => "4725360918"[parts[0].Count(r.Contains) / 2 % 15 % 11])
                ).Count(c => part1Matches.Contains(c))
            ).ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Select(line => line.Split('|'))
            .Sum(parts => int.Parse(
                string.Join("", parts[1]
                    .Trim()
                    .Split(' ')
                    .Select(r => "4725360918"[parts[0].Count(r.Contains) / 2 % 15 % 11])
                ))
            ).ToString();
    }
}