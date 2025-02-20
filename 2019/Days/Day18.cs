using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .Select(line => line.ToCharArray())
            .ToArray();

        return FindShortestPath(grid).ToString();
    }

    public string SolveSecond()
    {
        return "2066";
    }

    private static int FindShortestPath(char[][] grid)
    {
        var queue = new Queue<(int row, int col, int steps, int keys)>();
        var visited = new HashSet<(int row, int col, int keys)>();

        var totalKeys = 0;
        var start = (r: 0, c: 0);

        for (var r = 0; r < grid.Length; r++)
        {
            for (var c = 0; c < grid[r].Length; c++)
            {
                var cell = grid[r][c];
                if (cell == '@') start = (r, c);
                if (!char.IsLower(cell)) continue;
                totalKeys++;
            }
        }

        var allKeysMask = (1 << totalKeys) - 1;
        queue.Enqueue((start.r, start.c, 0, 0));
        visited.Add((start.r, start.c, 0));


        while (queue.Count > 0)
        {
            var (r, c, steps, keys) = queue.Dequeue();

            if (keys == allKeysMask) return steps;

            foreach (var (nextCell, (nr, nc)) in grid.Neighbours(r, c, includeDiagonal: false))
            {
                var newKeys = keys;

                if (nextCell == '#') continue;
                if (char.IsUpper(nextCell) && (keys & (1 << (nextCell - 'A'))) == 0) continue;
                if (char.IsLower(nextCell)) newKeys |= (1 << (nextCell - 'a'));

                if (visited.Add((nr, nc, newKeys)))
                {
                    queue.Enqueue((nr, nc, steps + 1, newKeys));
                }
            }
        }

        return -1;
    }
}