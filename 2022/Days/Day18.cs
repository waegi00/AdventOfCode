using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var cubes = this.InputToLines()
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(x => (x: x[0], y: x[1], z: x[2]))
            .ToHashSet();

        return cubes.Sum(c =>
                GetSides(c).Count(s => !cubes.Contains(s)))
            .ToString();
    }

    public string SolveSecond()
    {
        var cubes = this.InputToLines()
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(x => (x: x[0], y: x[1], z: x[2]))
            .ToHashSet();

        var seen = new HashSet<(int x, int y, int z)>();
        var todo = new Stack<(int, int, int)>();
        todo.Push((-1, -1, -1));

        while (todo.TryPop(out var here))
        {
            var neighbors = GetSides(here)
                .Except(cubes)
                .Except(seen)
                .Where(s => s.x is >= -1 and <= 25 && s.y is >= -1 and <= 25 && s.z is >= -1 and <= 25)
                .ToList();

            foreach (var s in neighbors)
            {
                todo.Push(s);
            }

            seen.Add(here);
        }

        return cubes.Sum(c =>
                GetSides(c).Count(s => seen.Contains(s)))
            .ToString();
    }

    private static HashSet<(int x, int y, int z)> GetSides((int x, int y, int z) pos)
    {
        return [
            (pos.x + 1, pos.y, pos.z), (pos.x - 1, pos.y, pos.z),
            (pos.x, pos.y + 1, pos.z), (pos.x, pos.y - 1, pos.z),
            (pos.x, pos.y, pos.z + 1), (pos.x, pos.y, pos.z - 1)
        ];
    }
}