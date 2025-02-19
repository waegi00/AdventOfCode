using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .Select(x => x.ToArray())
            .ToArray();

        var asteroids = grid.FindAll('#').ToList();

        var max = 0;

        foreach (var c in asteroids.Select(a => asteroids
                     .Where(x => x != a)
                     .Select(x => Angle(x, a))
                     .Distinct()
                     .Count()).Where(c => c > max))
        {
            max = c;
        }

        return max.ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .Select(x => x.ToArray())
            .ToArray();

        var asteroids = grid.FindAll('#')
            .Select(item => (x: item.j, y: item.i))
            .ToList();

        var result = (x: 0, y: 0);
        var max = 0;

        foreach (var a in asteroids)
        {
            var c = asteroids
                .Where(x => x != a)
                .Select(x => Angle(x, a))
                .Distinct()
                .Count();

            if (c <= max) continue;
            max = c;
            result = a;
        }

        asteroids.Remove(result);
        var angles = asteroids
            .Select(x => (pos: x, angle: Angle(result, x)))
            .OrderBy(x => x.angle)
            .ThenBy(x => Math.Abs(result.x - x.pos.x) + Math.Abs(result.y - x.pos.y))
            .ToList();

        var count = 0;
        double? lastAngle = null;
        (int x, int y) lastVaporized = (0, 0);
        var idx = 0;

        while (count < 200 && angles.Count > 0)
        {
            if (idx >= angles.Count)
            {
                idx = 0;
                lastAngle = null;
            }

            if (lastAngle == angles[idx].angle)
            {
                idx++;
                continue;
            }

            lastVaporized = angles[idx].pos;
            lastAngle = angles[idx].angle;
            angles.RemoveAt(idx);
            count++;
        }

        return (lastVaporized.x * 100 + lastVaporized.y).ToString();
    }

    private static double Angle((int x, int y) start, (int x, int y) end)
    {
        var angle = Math.Atan2(end.x - start.x, start.y - end.y) * 180 / Math.PI;
        return angle < 0 ? angle + 360 : angle;
    }
}