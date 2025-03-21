using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day23 : IRiddle
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
        var elves = this.InputToLines()
            .WithIndex()
            .SelectMany(x => x.item.WithIndex()
                .Where(y => y.item == '#')
                .Select(y => (x: x.index, y: y.index)))
            .ToHashSet();

        var checks = new (int dx, int dy)[4][];
        checks[0] = [(-1, -1), (-1, 0), (-1, 1)];
        checks[1] = [(1, -1), (1, 0), (1, 1)];
        checks[2] = [(-1, -1), (0, -1), (1, -1)];
        checks[3] = [(-1, 1), (0, 1), (1, 1)];

        var moved = true;
        var i = 0;
        while (isPart2 && moved || i < 10)
        {
            moved = false;
            var movements = new Dictionary<(int x, int y), List<(int x, int y)>>();

            foreach (var (x, y) in elves.Where(e => NextToElf(elves, e.x, e.y)))
            {
                foreach (var check in checks)
                {
                    if (check.Any(c => elves.Contains((x + c.dx, y + c.dy)))) continue;
                    var (dx, dy) = check[1];
                    movements.TryAdd((x + dx, y + dy), []);
                    movements[(x + dx, y + dy)].Add((x, y));
                    break;
                }
            }

            foreach (var (key, value) in movements
                         .Where(x => x.Value.Count == 1)
                         .Select(x => (x.Key, x.Value.First())))
            {
                elves.Remove(value);
                elves.Add(key);
                moved = true;
            }

            checks = checks.Rotate(3);
            i++;
        }

        return isPart2 ? i : (elves.Max(x => x.x) - elves.Min(x => x.x) + 1) * (elves.Max(x => x.y) - elves.Min(x => x.y) + 1) - elves.Count;
    }

    private static bool NextToElf(HashSet<(int, int)> elves, int x, int y)
    {
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                if (elves.Contains((x + dx, y + dy)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}