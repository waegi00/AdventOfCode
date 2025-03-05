using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        return SlidingCount(1).ToString();
    }

    public string SolveSecond()
    {
        return SlidingCount(3).ToString();
    }

    private int SlidingCount(int n)
    {
        var numbers = this.InputToLines()
            .Select(int.Parse)
            .ToArray();

        return numbers
            .Zip(numbers.Skip(n), (x, y) => x < y ? 1 : 0)
            .Sum();
    }
}