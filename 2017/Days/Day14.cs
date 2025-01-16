using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        const int size = 128;
        var grid = new bool[size][];
        for (var i = 0; i < size; i++)
        {
            grid[i] = new bool[size];
            var hash = string.Concat(
                Day10.KnotHash($"{input}-{i}".Select(x => (byte)x).ToList())
                    .Where(x => x != '\0')
                    .Select(hexDigit =>
                        Convert.ToString(Convert.ToInt32(hexDigit.ToString(), 16), 2)
                            .PadLeft(4, '0')));

            foreach (var (c, j) in hash.Select((x, index) => (x, index)))
            {
                grid[i][j] = c == '1';
            }
        }

        return grid.Sum(x => x.Count(y => y)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        const int size = 128;
        var grid = new bool[size][];
        for (var i = 0; i < size; i++)
        {
            grid[i] = new bool[size];
            var hash = string.Concat(
                Day10.KnotHash($"{input}-{i}".Select(x => (byte)x).ToList())
                    .Where(x => x != '\0')
                    .Select(hexDigit =>
                        Convert.ToString(Convert.ToInt32(hexDigit.ToString(), 16), 2)
                            .PadLeft(4, '0')));

            foreach (var (c, j) in hash.Select((x, index) => (x, index)))
            {
                grid[i][j] = c == '1';
            }
        }

        var regions = 0;
        while (grid.Any(x => x.Any(y => y)))
        {
            regions++;

            var visited = new HashSet<(int, int)>();
            var toVisit = new Queue<(int x, int y)>();
            toVisit.Enqueue(grid.FindFirst(true));

            while (toVisit.TryDequeue(out var curr))
            {
                if (!visited.Add(curr))
                {
                    continue;
                }

                grid[curr.x][curr.y] = false;

                foreach (var (on, (nx, ny)) in grid.Neighbours(curr.x, curr.y, includeDiagonal: false))
                {
                    if (!on || visited.Contains((nx, ny)))
                    {
                        continue;
                    }

                    toVisit.Enqueue((nx, ny));
                }
            }
        }

        return regions.ToString();
    }
}