using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines()
            .Sum(int.Parse)
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(int.Parse)
            .ToArray();

        var seen = new HashSet<int>();
        var i = 0;
        var curr = 0;

        do
        {
            curr += input[i++ % input.Length];
        } while (seen.Add(curr));

        return curr.ToString();
    }
}