using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2018.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        for (var t = 0; t < 10; t++)
        {
            var newGrid = grid.Select(x => x.ToArray()).ToArray();

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var neighbours = grid.Neighbours(i, j).ToList();

                    switch (grid[i][j])
                    {
                        case '#':
                            if (neighbours.All(x => x.Item1 != '|') ||
                                neighbours.All(x => x.Item1 != '#'))
                            {
                                newGrid[i][j] = '.';
                            }
                            break;
                        case '|':
                            if (neighbours.Count(x => x.Item1 == '#') >= 3)
                            {
                                newGrid[i][j] = '#';
                            }
                            break;
                        case '.':
                            if (neighbours.Count(x => x.Item1 == '|') >= 3)
                            {
                                newGrid[i][j] = '|';
                            }
                            break;
                    }
                }
            }

            grid = newGrid;
        }

        return (grid.Sum(x => x.Count(y => y == '#')) * grid.Sum(x => x.Count(y => y == '|')))
            .ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var seen = new Dictionary<char[][], int>(new JaggedArrayComparer<char>());
        const int goal = 1000000000;

        for (var t = 0; t < goal; t++)
        {
            var newGrid = grid.Select(x => x.ToArray()).ToArray();

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var neighbours = grid.Neighbours(i, j).ToList();

                    switch (grid[i][j])
                    {
                        case '#':
                            if (neighbours.All(x => x.Item1 != '|') ||
                                neighbours.All(x => x.Item1 != '#'))
                            {
                                newGrid[i][j] = '.';
                            }
                            break;
                        case '|':
                            if (neighbours.Count(x => x.Item1 == '#') >= 3)
                            {
                                newGrid[i][j] = '#';
                            }
                            break;
                        case '.':
                            if (neighbours.Count(x => x.Item1 == '|') >= 3)
                            {
                                newGrid[i][j] = '|';
                            }
                            break;
                    }
                }
            }

            grid = newGrid;
            if (seen.TryAdd(grid, t))
            {
                continue;
            }

            var prev = seen[grid];

            grid = seen.FirstOrDefault(x => x.Value == prev - 1 + (goal - t) % (t - prev)).Key;
            break;
        }

        return (grid.Sum(x => x.Count(y => y == '#')) * grid.Sum(x => x.Count(y => y == '|')))
            .ToString();
    }
}