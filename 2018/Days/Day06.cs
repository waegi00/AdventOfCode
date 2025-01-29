using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(", ").Select(int.Parse).ToArray())
            .Select((x, i) => (X: x[0], Y: x[1], Index: i))
            .ToArray();

        var minX = input.Min(point => point.X) - 1;
        var maxX = input.Max(point => point.X) + 1;
        var minY = input.Min(point => point.Y) - 1;
        var maxY = input.Max(point => point.Y) + 1;
        var grid = new Dictionary<(int, int), int>();

        for (var x = minX; x < maxX; x++)
        {
            for (var y = minY; y < maxY; y++)
            {
                var min = input.Min(item => Math.Abs(item.X - x) + Math.Abs(item.Y - y));

                foreach (var item in input)
                {
                    if (Math.Abs(item.X - x) + Math.Abs(item.Y - y) != min) continue;
                    if (grid.ContainsKey((x, y)))
                    {
                        grid[(x, y)] = -1;
                        break;
                    }
                    grid[(x, y)] = item.Index;
                }
            }
        }

        var s = new HashSet<int> { -1 };
        s.UnionWith(grid.Where(kvp => kvp.Key.Item2 == maxY - 1).Select(kvp => kvp.Value));
        s.UnionWith(grid.Where(kvp => kvp.Key.Item2 == minY).Select(kvp => kvp.Value));
        s.UnionWith(grid.Where(kvp => kvp.Key.Item1 == maxX - 1).Select(kvp => kvp.Value));
        s.UnionWith(grid.Where(kvp => kvp.Key.Item1 == minX).Select(kvp => kvp.Value));

        return grid.Values
            .GroupBy(x => x)
            .OrderByDescending(x => x.Count())
            .First(x => !s.Contains(x.Key))
            .Count()
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(", ").Select(int.Parse).ToArray())
            .Select((x, i) => (X: x[0], Y: x[1], Index: i))
            .ToArray();

        var minX = input.Min(point => point.X) - 1;
        var maxX = input.Max(point => point.X) + 1;
        var minY = input.Min(point => point.Y) - 1;
        var maxY = input.Max(point => point.Y) + 1;

        return Enumerable.Range(minX, maxX - minX)
            .SelectMany(x => Enumerable.Range(minY, maxY - minY)
                .Where(y => input.Sum(item => Math.Abs(x - item.X) + Math.Abs(y - item.Y)) < 10000))
            .Count()
            .ToString();
    }
}