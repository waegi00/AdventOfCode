using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2019.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var ratings = new HashSet<int>();

        var last = Rating(grid);
        while (ratings.Add(last))
        {
            grid = Transform(grid);
            last = Rating(grid);
        }

        return last.ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var bugs = new HashSet<(int level, int x, int y)>();
        for (var x = 0; x < grid.Length; x++)
        {
            for (var y = 0; y < grid[x].Length; y++)
            {
                if (grid[x][y] != '#') continue;
                bugs.Add((0, x, y));
            }
        }

        for (var i = 0; i < 200; i++)
        {
            bugs = Step(bugs);
        }

        return bugs.Count.ToString();
    }

    private static char[][] Transform(char[][] grid)
    {
        var newGrid = grid.Select(x => x.ToArray()).ToArray();

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                var bugNeighbours = grid.Neighbours(i, j, includeDiagonal: false)
                    .Count(x => x.Item1 == '#');

                if (grid[i][j] == '#')
                {
                    newGrid[i][j] = bugNeighbours == 1 ? '#' : '.';
                }
                else
                {
                    newGrid[i][j] = bugNeighbours is 1 or 2 ? '#' : '.';
                }
            }
        }

        return newGrid;
    }

    private static int Rating(char[][] grid)
    {
        var n = 0;

        for (var i = grid.Length - 1; i >= 0; i--)
        {
            for (var j = grid[i].Length - 1; j >= 0; j--)
            {
                n |= grid[i][j] == '#' ? 1 : 0;
                n <<= 1;
            }
        }

        return n >> 1;
    }

    private static HashSet<(int, int, int)> Step(HashSet<(int, int, int)> bugs)
    {
        var minLevel = int.MaxValue;
        var maxLevel = int.MinValue;

        foreach (var bug in bugs)
        {
            minLevel = Math.Min(minLevel, bug.Item1);
            maxLevel = Math.Max(maxLevel, bug.Item1);
        }

        var bugCount = new HashSet<(int, int, int)>();
        for (var lev = minLevel - 1; lev <= maxLevel + 1; lev++)
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (i == 2 && j == 2) continue;

                    if (bugs.Contains((lev, i, j)) && CountAdj(lev, i, j) == 1)
                    {
                        bugCount.Add((lev, i, j));
                    }
                    if (!bugs.Contains((lev, i, j)) && (CountAdj(lev, i, j) == 1 || CountAdj(lev, i, j) == 2))
                    {
                        bugCount.Add((lev, i, j));
                    }
                }
            }
        }
        return bugCount;

        int CountAdj(int l, int x, int y)
        {
            var adj = new List<(int, int, int)>
            {
                (l, x - 1, y),
                (l, x + 1, y),
                (l, x, y - 1),
                (l, x, y + 1),
            };

            var adjacentTiles = new List<(int, int, int)>();
            foreach (var aa in adj)
            {
                var la = aa.Item1;
                var xa = aa.Item2;
                var ya = aa.Item3;

                switch (xa)
                {
                    case -1:
                        adjacentTiles.Add((la - 1, 1, 2));
                        break;
                    case 2 when ya == 2 && x == 1:
                        adjacentTiles.Add((la + 1, 0, 0));
                        adjacentTiles.Add((la + 1, 0, 1));
                        adjacentTiles.Add((la + 1, 0, 2));
                        adjacentTiles.Add((la + 1, 0, 3));
                        adjacentTiles.Add((la + 1, 0, 4));
                        break;
                    case 2 when ya == 2 && x == 3:
                        adjacentTiles.Add((la + 1, 4, 0));
                        adjacentTiles.Add((la + 1, 4, 1));
                        adjacentTiles.Add((la + 1, 4, 2));
                        adjacentTiles.Add((la + 1, 4, 3));
                        adjacentTiles.Add((la + 1, 4, 4));
                        break;
                    case 5:
                        adjacentTiles.Add((la - 1, 3, 2));
                        break;
                    default:
                        {
                            switch (ya)
                            {
                                case -1:
                                    adjacentTiles.Add((la - 1, 2, 1));
                                    break;
                                case 2 when xa == 2 && y == 1:
                                    adjacentTiles.Add((la + 1, 0, 0));
                                    adjacentTiles.Add((la + 1, 1, 0));
                                    adjacentTiles.Add((la + 1, 2, 0));
                                    adjacentTiles.Add((la + 1, 3, 0));
                                    adjacentTiles.Add((la + 1, 4, 0));
                                    break;
                                case 2 when xa == 2 && y == 3:
                                    adjacentTiles.Add((la + 1, 0, 4));
                                    adjacentTiles.Add((la + 1, 1, 4));
                                    adjacentTiles.Add((la + 1, 2, 4));
                                    adjacentTiles.Add((la + 1, 3, 4));
                                    adjacentTiles.Add((la + 1, 4, 4));
                                    break;
                                case 5:
                                    adjacentTiles.Add((la - 1, 2, 3));
                                    break;
                                default:
                                    adjacentTiles.Add(aa);
                                    break;
                            }

                            break;
                        }
                }
            }

            return adjacentTiles.Count(bugs.Contains);
        }
    }
}