using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();

        const int length = 71;
        const int width = 71;
        var grid = new bool[length][];
        for (var i = 0; i < length; i++)
        {
            grid[i] = new bool[width];
        }

        const int take = 1024;
        foreach (var item in input.Take(take))
        {
            grid[item[0]][item[1]] = true;
        }

        return ShortestPath(grid).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();

        const int length = 71;
        const int width = 71;
        var grid = new bool[length][];
        for (var i = 0; i < length; i++)
        {
            grid[i] = new bool[width];
        }

        foreach (var item in input)
        {
            grid[item[0]][item[1]] = true;
            if (ShortestPath(grid) == -1)
            {
                return $"{item[0]},{item[1]}";
            }
        }

        return "";
    }

    private static int ShortestPath(bool[][] grid)
    {
        var n = grid.Length;
        var m = grid[0].Length;

        if (grid[0][0] || grid[n - 1][m - 1])
        {
            return -1;
        }

        var directions = new int[][] { [0, 1], [1, 0], [0, -1], [-1, 0] };

        var queue = new Queue<(int, int, int)>();
        queue.Enqueue((0, 0, 0));

        var visited = new bool[n][];
        for (var i = 0; i < n; i++)
        {
            visited[i] = new bool[m];
        }

        visited[0][0] = true;

        while (queue.Count > 0)
        {
            var (x, y, dist) = queue.Dequeue();

            if (x == n - 1 && y == m - 1)
            {
                return dist;
            }

            foreach (var dir in directions)
            {
                var newX = x + dir[0];
                var newY = y + dir[1];

                if (!grid.IsValidPosition(newX, newY) || grid[newX][newY] || visited[newX][newY])
                {
                    continue;
                }
                visited[newX][newY] = true;
                queue.Enqueue((newX, newY, dist + 1));
            }
        }

        return -1;
    }
}