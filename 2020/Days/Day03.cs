using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2020.Days;

public class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var (x, y) = (0, 0);
        var (dx, dy) = (3, 1);

        var trees = 0;

        while (grid.IsValidPosition(y + dy, (x + dx) % grid[y].Length))
        {
            x += dx;
            y += dy;

            if (grid[y][x % grid[y].Length] == '#')
            {
                trees++;
            }
        }

        return trees.ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var trees = new List<long>();
        foreach (var (dx, dy) in new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) })
        {
            var tree = 0;
            var (x, y) = (0, 0);

            while (grid.IsValidPosition(y + dy, (x + dx) % grid[y].Length))
            {
                x += dx;
                y += dy;

                if (grid[y][x % grid[y].Length] == '#')
                {
                    tree++;
                }
            }

            trees.Add(tree);
        }

        return trees.Product().ToString();
    }
}