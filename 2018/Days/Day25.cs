using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var points = this.InputToLines()
            .Select(x => x.Split(',').Select(int.Parse).ToArray())
            .Select(x => new Point4D(x[0], x[1], x[2], x[3]))
            .ToList();

        var constellations = new List<List<Point4D>>();
        while (points.Count > 0)
        {
            var p = points[0];
            points.RemoveAt(0);

            var indices = new List<int>();
            for (var i = 0; i < constellations.Count; i++)
            {
                if (constellations[i].Any(c => c.InSameConstellation(p)))
                {
                    indices.Add(i);
                }
            }

            MergeConstellations(ref constellations, indices, p);
        }

        return constellations.Count.ToString();
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }

    private static void MergeConstellations(ref List<List<Point4D>> constellations, List<int> indices, Point4D p)
    {
        if (indices.Count == 0)
        {
            constellations.Add([p]);
            return;
        }

        var mergedConstellation = constellations
            .Where((_, i) => indices.Contains(i))
            .SelectMany(x => x)
            .Union([p])
            .ToList();

        constellations = constellations.Where((_, i) => !indices.Contains(i)).ToList();

        constellations.Add(mergedConstellation);
    }

    private record Point4D(int X, int Y, int Z, int W)
    {
        public bool InSameConstellation(Point4D other)
        {
            return ManhattanDistance(other) <= 3;
        }

        private int ManhattanDistance(Point4D other)
        {
            return Math.Abs(X - other.X) +
                   Math.Abs(Y - other.Y) +
                   Math.Abs(Z - other.Z) +
                   Math.Abs(W - other.W);
        }
    }
}