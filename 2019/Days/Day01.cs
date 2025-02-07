using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines()
            .Select(int.Parse)
            .Select(x => x / 3 - 2)
            .Sum()
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Select(int.Parse)
            .Select(Sum)
            .Sum()
            .ToString();
    }

    private static int Sum(int x)
    {
        var sum = 0;

        while (x / 3 - 2 > 0)
        {
            x = x / 3 - 2;
            sum += x;
        }

        return sum;
    }
}