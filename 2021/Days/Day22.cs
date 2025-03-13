using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(-50, 50).ToString();
    }

    public string SolveSecond()
    {
        return Solve(null, null).ToString();
    }

    private long Solve(int? min, int? max)
    {
        var cuboids = this.InputToLines()
            .Select(s => s.Replace("x=", "").Replace(",y=", " ").Replace(",z=", " ").Replace("..", " "))
            .Select(s => s.Replace("on", "1").Replace("off", "0"))
            .Select(s => s.Split().Select(int.Parse).ToList())
            .ToList();

        var cores = new List<List<int>>();

        foreach (var cuboid in cuboids)
        {
            var toAdd = cuboid[0] == 1 ? new List<List<int>> { cuboid } : new List<List<int>>();
            toAdd.AddRange(cores.Select(core => Intersection(cuboid, core)).OfType<List<int>>());

            cores.AddRange(toAdd);
        }

        return CountOnCubes(cores, min, max);
    }

    private static List<int>? Intersection(List<int> s, List<int> t)
    {
        var mm = new Func<int, int, int>[] { (_, b) => -b, Math.Max, Math.Min, Math.Max, Math.Min, Math.Max, Math.Min };
        var n = s.Select((value, i) => mm[i](value, t[i])).ToList();

        return n[1] > n[2] || n[3] > n[4] || n[5] > n[6] ? null : n;
    }

    private static long CountOnCubes(List<List<int>> cores, int? min, int? max)
    {
        long onCount = 0;

        foreach (var c in cores)
        {
            var xMin = min != null ? Math.Max(c[1], min.Value) : c[1];
            var xMax = max != null ? Math.Min(c[2], max.Value) : c[2];
            var yMin = min != null ? Math.Max(c[3], min.Value) : c[3];
            var yMax = max != null ? Math.Min(c[4], max.Value) : c[4];
            var zMin = min != null ? Math.Max(c[5], min.Value) : c[5];
            var zMax = max != null ? Math.Min(c[6], max.Value) : c[6];

            if (xMin <= xMax && yMin <= yMax && zMin <= zMax)
            {
                onCount += c[0] * (long)(xMax - xMin + 1) * (yMax - yMin + 1) * (zMax - zMin + 1);
            }
        }

        return onCount;
    }
}