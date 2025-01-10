using System.Numerics;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var num = int.Parse(this.InputToText());

        (int, int)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        var visited = new HashSet<(int x, int y)>();
        var toVisit = new Queue<(int x, int y, int l)>();
        toVisit.Enqueue((1, 1, 0));

        while (toVisit.TryDequeue(out var pos))
        {
            visited.Add((pos.x, pos.y));

            if (pos is { x: 31, y: 39 })
            {
                return pos.l.ToString();
            }

            foreach (var (dx, dy) in directions)
            {
                var nx = pos.x + dx;
                var ny = pos.y + dy;
                if (nx >= 0 && ny >= 0 && !visited.Contains((nx, ny)) && IsOpenSpace(nx, ny, num))
                {
                    toVisit.Enqueue((nx, ny, pos.l + 1));
                }
            }
        }

        return "";
    }

    public string SolveSecond()
    {
        var num = int.Parse(this.InputToText());

        (int, int)[] directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        var visited = new HashSet<(int x, int y)>();
        var toVisit = new Queue<(int x, int y, int l)>();
        toVisit.Enqueue((1, 1, 0));

        while (toVisit.TryDequeue(out var pos))
        {
            visited.Add((pos.x, pos.y));

            if (pos.l == 50) continue;

            foreach (var (dx, dy) in directions)
            {
                var nx = pos.x + dx;
                var ny = pos.y + dy;
                if (nx >= 0 && ny >= 0 && !visited.Contains((nx, ny)) && IsOpenSpace(nx, ny, num))
                {
                    toVisit.Enqueue((nx, ny, pos.l + 1));
                }
            }
        }

        return visited.Count.ToString();
    }

    private static bool IsOpenSpace(int x, int y, int num)
    {
        return BitOperations.PopCount(Convert.ToUInt64(x * x + 3 * x + 2 * x * y + y + y * y + num)) % 2 == 0;
    }
}
