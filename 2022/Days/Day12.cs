using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2022.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        return Solve().ToString();
    }

    public string SolveSecond()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPart2 = false)
    {

        var grid = this.InputToLines().ToCharArray();

        var start = grid.FindFirst('S');
        var goal = grid.FindFirst('E');

        var seen = new HashSet<(int, int)>();
        var toVisit = new PriorityQueue<(int, int), int>();
        toVisit.Enqueue(start, 0);

        grid[start.i][start.j] = 'a';
        grid[goal.i][goal.j] = 'z';

        while (toVisit.TryDequeue(out var curr, out var distance))
        {
            if (curr == goal) return distance;
            if (!seen.Add(curr)) continue;

            foreach (var (c, n) in grid.Neighbours(curr.Item1, curr.Item2, includeDiagonal: false))
            {
                if (c - grid[curr.Item1][curr.Item2] <= 1)
                {
                    toVisit.Enqueue(n, distance + (isPart2 && c == 'a' ? 0 : 1));
                }
            }
        }

        return -1;
    }
}