using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day22 : IRiddle
{
    private static int depth;
    private static (int x, int y) target;
    private static readonly Dictionary<(int x, int y), int> cache = [];

    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(": ")[1].Split(',').Select(int.Parse).ToArray())
            .ToArray();

        depth = input[0][0];
        target = (input[1][0], input[1][1]);
        cache.Clear();

        return Enumerable.Range(0, target.x + 1)
            .Sum(x => Enumerable.Range(0, target.y + 1)
                .Sum(y => GetRegionType((x, y))))
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(": ")[1].Split(',').Select(int.Parse).ToArray())
            .ToArray();

        depth = input[0][0];
        target = (input[1][0], input[1][1]);
        cache.Clear();

        var toVisit = new PriorityQueue<(int x, int y, char tool), int>(Comparer<int>.Create((a, b) => a - b));
        var visited = new HashSet<(int x, int y, char tool)>();

        toVisit.Enqueue((0, 0, 'T'), 0);

        while (toVisit.TryDequeue(out var item, out var t))
        {
            if ((item.x, item.y) == target && item.tool == 'T')
            {
                return t.ToString();
            }

            if (!visited.Add(item))
            {
                continue;
            }

            foreach (var (dx, dy) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
            {
                var nx = item.x + dx;
                var ny = item.y + dy;
                if (nx < 0 || ny < 0)
                {
                    continue;
                }

                if (GetTools(GetRegionType((nx, ny))).Contains(item.tool))
                {
                    toVisit.Enqueue((nx, ny, item.tool), t + 1);
                }
            }

            foreach (var tool in GetTools(GetRegionType((item.x, item.y))))
            {
                toVisit.Enqueue(item with { tool = tool }, t + 7);
            }
        }

        return "";
    }

    private static int GetRegionType((int x, int y) pos)
    {
        return ErosionLevel(pos) % 3;
    }

    private static int ErosionLevel((int x, int y) pos)
    {
        if (cache.TryGetValue(pos, out var value))
        {
            return value;
        }

        value = (GeologicalIndex(pos) + depth) % 20183;
        cache.Add(pos, value);

        return value;
    }

    private static int GeologicalIndex((int x, int y) pos)
    {
        if (pos == (0, 0) || pos == target)
        {
            return 0;
        }

        if (pos.y == 0)
        {
            return pos.x * 16807;
        }

        if (pos.x == 0)
        {
            return pos.y * 48271;
        }

        return ErosionLevel((pos.x - 1, pos.y)) * ErosionLevel((pos.x, pos.y - 1));
    }

    private static List<char> GetTools(int region)
    {
        return region switch
        {
            0 => ['C', 'T'],
            1 => ['C', 'N'],
            2 => ['T', 'N'],
            _ => throw new ArgumentOutOfRangeException(nameof(region), region, null)
        };
    }
}