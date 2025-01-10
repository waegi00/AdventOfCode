using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day20 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split('-'))
            .Select(x => x.Select(uint.Parse).ToList())
            .Select(x => (start: x[0], end: x[1]))
            .OrderBy(x => x.start)
            .ThenBy(x => x.end)
            .ToList();

        var result = 0u;
        foreach (var (start, end) in input)
        {
            if (result < start)
            {
                break;
            }

            result = Math.Max(result, end + 1);
        }

        return result.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split('-'))
            .Select(x => x.Select(uint.Parse).ToList())
            .Select(x => (start: x[0], end: x[1]))
            .OrderBy(x => x.start)
            .ThenBy(x => x.end)
            .ToList();

        var result = 0u;
        var curr = 0u;

        foreach (var (start, end) in input)
        {
            if (curr < start)
            {
                result += start - curr;
            }

            curr = Math.Max(curr, Math.Max(end + 1, end));
        }

        result += uint.MaxValue - curr;

        return result.ToString();
    }
}