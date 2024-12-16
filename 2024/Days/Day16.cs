using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .Select(x => x.ToCharArray()).ToArray();

        var start = grid.FindFirst('S');
        var end = grid.FindFirst('E');

        var costs = new HashSet<(int x, int y)>();
        var min = int.MaxValue;

        var toVisit = new PriorityQueue<(int x, int y, Direction dir), int>();

        toVisit.Enqueue((start.i, start.j, Direction.Right), 0);

        while (toVisit.TryDequeue(out var item, out var cost))
        {
            costs.Add((item.x, item.y));
            if (item.x == end.i && item.y == end.j)
            {
                min = Math.Min(min, cost);
            }

            var neighbours =
                grid.Neighbours(item.x, item.y, true, true, false)
                .Where(n => n.Item1 != '#').ToList();
            foreach (var (_, (nx, ny)) in neighbours)
            {
                if (costs.Contains((nx, ny)))
                {
                    continue;
                }

                var dir = Dir(nx - item.x, ny - item.y);
                var c = cost + (dir == item.dir ? 1 : 1001);
                toVisit.Enqueue((nx, ny, dir), c);
            }
        }

        return min.ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .Select(x => x.ToCharArray()).ToArray();

        var start = grid.FindFirst('S');
        var end = grid.FindFirst('E');

        var costs = new Dictionary<(int x, int y, Direction dir), int>();
        var min = int.MaxValue;
        var mins = new HashSet<(int, int)>();

        var toVisit = new PriorityQueue<(int x, int y, Direction dir, HashSet<(int, int)> history), int>();

        toVisit.Enqueue((start.i, start.j, Direction.Right, []), 0);

        while (toVisit.TryDequeue(out var item, out var cost))
        {
            if (!costs.TryAdd((item.x, item.y, item.dir), cost))
            {
                if (costs[(item.x, item.y, item.dir)] < cost)
                {
                    continue;
                }
            }
            if (item.x == end.i && item.y == end.j)
            {
                if (cost == min)
                {
                    mins.UnionWith(item.history);
                }
                else if (cost < min)
                {
                    mins = item.history;
                    min = cost;
                }
            }

            var neighbours =
                grid.Neighbours(item.x, item.y, true, true, false)
                    .Where(n => n.Item1 != '#').ToList();
            foreach (var (_, (nx, ny)) in neighbours)
            {
                var dir = Dir(nx - item.x, ny - item.y);
                var c = cost + (dir == item.dir ? 1 : 1001);
                var set = new HashSet<(int, int)>(item.history) { (item.x, item.y) };
                toVisit.Enqueue((nx, ny, dir, set), c);
            }
        }

        return (mins.Count + 1).ToString();
    }

    private enum Direction { Up, Right, Down, Left };

    private static Direction Dir(int dx, int dy)
    {
        return (dx, dy) switch
        {
            (-1, 0) => Direction.Up,
            (0, 1) => Direction.Right,
            (1, 0) => Direction.Down,
            (0, -1) => Direction.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}