using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2015.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .ToCharArray()
            .Select(x => x.Select(y => y == '#').ToArray())
            .ToArray();

        for (var n = 0; n < 100; n++)
        {
            var newGrid = grid.Select(x => x.ToArray()).ToArray();

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var val = false;
                    var ns = grid.Neighbours(i, j);
                    var on = ns.Count(neighbour => neighbour.Item1);

                    if (grid[i][j] && on is >= 2 and <= 3 || !grid[i][j] && on == 3)
                    {
                        val = true;
                    }

                    newGrid[i][j] = val;
                }
            }

            grid = newGrid;
        }

        return grid.SelectMany(x => x).Sum(b => b ? 1 : 0).ToString();

    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .ToCharArray()
            .Select(x => x.Select(y => y == '#').ToArray())
            .ToArray();
        
        grid[0][0] = grid[0][^1] = grid[^1][0] = grid[^1][^1] = true;

        for (var n = 0; n < 100; n++)
        {
            var newGrid = grid.Select(x => x.ToArray()).ToArray();

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var val = false;
                    var ns = grid.Neighbours(i, j);
                    var on = ns.Count(neighbour => neighbour.Item1);

                    if (grid[i][j] && on is >= 2 and <= 3 || !grid[i][j] && on == 3)
                    {
                        val = true;
                    }

                    newGrid[i][j] = val;
                }
            }

            grid = newGrid;
            grid[0][0] = grid[0][^1] = grid[^1][0] = grid[^1][^1] = true;
        }


        return grid.SelectMany(x => x).Sum(b => b ? 1 : 0).ToString();

    }
}