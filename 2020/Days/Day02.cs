using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2020.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var passwords = this.InputToLines()
            .Select(x => x.Split([' ', '.', '-']))
            .Select(x => (from: int.Parse(x[0]), to: int.Parse(x[1]), c: x[2][0], s: x[3]))
            .ToList();

        return passwords.Count(x =>
            x.s.Count(y => y == x.c).IsBetween(x.from, x.to))
            .ToString();
    }

    public string SolveSecond()
    {
        var passwords = this.InputToLines()
            .Select(x => x.Split([' ', '.', '-']))
            .Select(x => (a: int.Parse(x[0]), b: int.Parse(x[1]), c: x[2][0], s: x[3]))
            .ToList();

        return passwords
            .Count(x => x.s[x.a - 1] == x.c ^ x.s[x.b - 1] == x.c)
            .ToString();
    }
}