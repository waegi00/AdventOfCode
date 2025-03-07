using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        return grid
            .WithIndex()
            .Sum(x => x.item
                .WithIndex()
                .Where(y => grid.Neighbours(x.index, y.index, includeDiagonal: false)
                    .All(n => n.Item1 > y.item))
                .Sum(y => y.item + 1))
            .ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        var lowPoints = grid
            .WithIndex()
            .SelectMany(x => x.item
                .WithIndex()
                .Where(y => grid.Neighbours(x.index, y.index, includeDiagonal: false)
                    .All(n => n.Item1 > y.item))
                .Select(y => (x.index, y.index))
                .ToArray())
            .ToArray();

        return lowPoints
            .Select(CountBasin)
            .OrderDescending()
            .Take(3)
            .Product()
            .ToString();

        int CountBasin((int x, int y) pos)
        {
            if (grid[pos.x][pos.y] == 9)
            {
                return 0;
            }

            grid[pos.x][pos.y] = 9;
            return 1 + grid.Neighbours(pos.x, pos.y, includeDiagonal: false)
                .Sum(x => CountBasin(x.Item2));
        }
    }
}