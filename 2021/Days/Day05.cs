using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var lines = this.InputToLines()
            .Select(x => x
                .Split([" -> ", ","], StringSplitOptions.None)
                .Select(int.Parse)
                .ToArray())
            .Select(x => (x1: x[0], y1: x[1], x2: x[2], y2: x[3]))
            .Where(x => x.x1 == x.x2 || x.y1 == x.y2)
            .ToArray();

        return CountOverlaps(lines).ToString();
    }

    public string SolveSecond()
    {
        var lines = this.InputToLines()
            .Select(x => x
                .Split([" -> ", ","], StringSplitOptions.None)
                .Select(int.Parse)
                .ToArray())
            .Select(x => (x1: x[0], y1: x[1], x2: x[2], y2: x[3]))
            .ToArray();

        return CountOverlaps(lines).ToString();
    }

    private static int CountOverlaps((int, int, int, int)[] lines)
    {
        var map = new Dictionary<(int, int), int>();

        foreach (var (x1, y1, x2, y2) in lines)
        {
            var dx = Math.Sign(x2 - x1);
            var dy = Math.Sign(y2 - y1);

            var x = x1;
            var y = y1;

            while (x != x2 + dx || y != y2 + dy)
            {
                map.TryAdd((x, y), 0);
                map[(x, y)]++;
                x += dx;
                y += dy;
            }
        }

        return map.Count(x => x.Value >= 2);
    }
}