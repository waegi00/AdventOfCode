using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n");

        var dots = input[0]
            .Split("\r\n")
            .Select(x => x.Split(','))
            .Select(x => (x: int.Parse(x[0]), y: int.Parse(x[1])))
            .ToHashSet();

        var (c, n) = input[1]
            .Split("\r\n")
            .Select(x => x.Split('='))
            .Select(x => (x[0][^1], int.Parse(x[1])))
            .First();

        dots = c switch
        {
            'y' => dots.Select(d => (d.x, d.y < n ? d.y : Math.Abs(2 * n - d.y))).ToHashSet(),
            'x' => dots.Select(d => (d.x < n ? d.x : Math.Abs(2 * n - d.x), d.y)).ToHashSet(),
            _ => dots
        };

        return dots.Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n");

        var dots = input[0]
            .Split("\r\n")
            .Select(x => x.Split(','))
            .Select(x => (x: int.Parse(x[0]), y: int.Parse(x[1])))
            .ToHashSet();

        var folds = input[1]
            .Split("\r\n")
            .Select(x => x.Split('='))
            .Select(x => (x[0][^1], int.Parse(x[1])))
            .ToArray();

        foreach (var (c, n) in folds)
        {
            dots = c switch
            {
                'y' => dots.Select(d => (d.x, d.y < n ? d.y : Math.Abs(2 * n - d.y))).ToHashSet(),
                'x' => dots.Select(d => (d.x < n ? d.x : Math.Abs(2 * n - d.x), d.y)).ToHashSet(),
                _ => dots
            };
        }

        Print(dots);
        return "";
    }

    private static void Print(HashSet<(int, int)> dots)
    {
        var minX = dots.Min(x => x.Item1);
        var maxX = dots.Max(x => x.Item1);
        var minY = dots.Min(x => x.Item2);
        var maxY = dots.Max(x => x.Item2);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(dots.Contains((x, y)) ? '#' : ' ');
            }

            Console.WriteLine();
        }
    }
}