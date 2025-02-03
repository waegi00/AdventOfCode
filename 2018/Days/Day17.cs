using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public partial class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var res = Run();

        var result = 0;
        for (var y = res.minY; y <= res.maxY; y++)
        {
            for (var x = 0; x < res.grid[0].Length; x++)
            {
                if (res.grid[y][x] == '|' || res.grid[y][x] == '~')
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    public string SolveSecond()
    {
        var res = Run();

        var result = 0;
        for (var y = res.minY; y <= res.maxY; y++)
        {
            for (var x = 0; x < res.grid[0].Length; x++)
            {
                if (res.grid[y][x] == '~')
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    private (char[][] grid, int minY, int maxY) Run()
    {
        var data = (from line in this.InputToLines()
                    let numbers = NumberRegex()
                        .Matches(line)
                        .Select(m => int.Parse(m.Value))
                        .ToArray()
                    select line[0] == 'x'
                        ? (numbers[0], numbers[0], numbers[1], numbers[2])
                        : (numbers[1], numbers[2], numbers[0], numbers[0])).ToList();

        var minX = data.Min(d => d.Item1);
        var maxX = data.Max(d => d.Item2);
        var minY = data.Min(d => d.Item3);
        var maxY = data.Max(d => d.Item4);

        var grid = new char[maxY + 1][];
        for (var i = 0; i <= maxY; i++)
        {
            grid[i] = Enumerable.Repeat('.', maxX - minX + 3).ToArray();
        }

        foreach (var (x1, x2, y1, y2) in data)
        {
            for (var x = x1; x <= x2; x++)
            {
                for (var y = y1; y <= y2; y++)
                {
                    grid[y][x - minX + 1] = '#';
                }
            }
        }

        var springX = 500 - minX + 1;
        grid[0][springX] = '+';
        Flow(grid, springX, 0, 0);

        return (grid, minY, maxY);
    }

    private static int Flow(char[][] grid, int x, int y, int d)
    {
        while (true)
        {
            if (grid[y][x] == '.')
            {
                grid[y][x] = '|';
            }

            if (y == grid.Length - 1)
            {
                return x;
            }

            if (grid[y][x] == '#')
            {
                return x;
            }

            if (grid[y + 1][x] == '.')
            {
                Flow(grid, x, y + 1, 0);
            }

            if (grid[y + 1][x] != '~' && grid[y + 1][x] != '#')
            {
                return x;
            }

            if (d != 0)
            {
                x += d;
                continue;
            }

            var leftX = Flow(grid, x - 1, y, -1);
            var rightX = Flow(grid, x + 1, y, 1);

            if (grid[y][leftX] != '#' || grid[y][rightX] != '#')
            {
                return x;
            }

            for (var fillX = leftX + 1; fillX < rightX; fillX++)
            {
                grid[y][fillX] = '~';
            }

            return x;
        }
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}