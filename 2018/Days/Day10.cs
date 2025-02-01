using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var points = this.InputToLines()
            .Select(x => x.Split(["=<", ",", ">"], StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new[] { x[1], x[2], x[4], x[5] }.Select(int.Parse).ToArray())
            .Select(x => new Point(x[0], x[1], x[2], x[3]))
            .ToArray();

        var minSize = long.MaxValue;

        while (true)
        {
            var newPoints = points.Select(p => p.Move()).ToArray();
            var maxX = newPoints.Max(p => p.x);
            var maxY = newPoints.Max(p => p.y);
            var minX = newPoints.Min(p => p.x);
            var minY = newPoints.Min(p => p.y);
            var size = Math.Abs(Math.Abs(maxX) - Math.Abs(minX)) * Math.Abs(Math.Abs(maxY) - Math.Abs(minY));

            if (size > minSize)
            {
                for (var j = points.Min(p => p.y); j <= points.Max(p => p.y); j++)
                {
                    for (var i = points.Min(p => p.x); i <= points.Max(p => p.x); i++)
                    {
                        Console.Write(points.Any(p => p.x == i && p.y == j) ? '#' : ' ');
                    }
                    Console.WriteLine();
                }

                break;
            }

            minSize = size;
            points = newPoints;
        }

        return "";
    }

    public string SolveSecond()
    {
        var points = this.InputToLines()
            .Select(x => x.Split(["=<", ",", ">"], StringSplitOptions.RemoveEmptyEntries))
            .Select(x => new[] { x[1], x[2], x[4], x[5] }.Select(int.Parse).ToArray())
            .Select(x => new Point(x[0], x[1], x[2], x[3]))
            .ToArray();

        var minSize = long.MaxValue;
        var t = 0;

        while (true)
        {
            var newPoints = points.Select(p => p.Move()).ToArray();
            var maxX = newPoints.Max(p => p.x);
            var maxY = newPoints.Max(p => p.y);
            var minX = newPoints.Min(p => p.x);
            var minY = newPoints.Min(p => p.y);
            var size = Math.Abs(Math.Abs(maxX) - Math.Abs(minX)) * Math.Abs(Math.Abs(maxY) - Math.Abs(minY));

            if (size > minSize)
            {
                break;
            }

            minSize = size;
            points = newPoints;
            t++;
        }

        return t.ToString();
    }

    private record Point(int x, int y, int vx, int vy)
    {
        public Point Move()
        {
            return this with { x = x + vx, y = y + vy };
        }
    }
}