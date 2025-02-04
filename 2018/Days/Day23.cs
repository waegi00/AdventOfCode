using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        var nanobots = this.InputToLines()
            .Select(x => x[5..].Split([">, r=", ","], StringSplitOptions.None).Select(int.Parse).ToArray())
            .Select(x => new Nanobot(new Position(x[0], x[1], x[2]), x[3]))
            .ToArray();

        return nanobots.MaxBy(x => x.R)!.Strength(nanobots).ToString();
    }

    public string SolveSecond()
    {
        var nanobots = this.InputToLines()
            .Select(x => x[5..].Split([">, r=", ","], StringSplitOptions.None).Select(int.Parse).ToArray())
            .Select(x => new Nanobot(new Position(x[0], x[1], x[2]), x[3]))
            .ToArray();

        var maxAbsCord = nanobots.Max(x => new[] { Math.Abs(x.Position.X) + x.R, Math.Abs(x.Position.Y) + x.R, Math.Abs(x.Position.Z) + x.R }.Max());
        var boxSize = 1;
        while (boxSize <= maxAbsCord)
        {
            boxSize *= 2;
        }

        var initialBox = (new Position(-boxSize, -boxSize, -boxSize), new Position(boxSize, boxSize, boxSize));

        var workHeap = new SortedSet<(int negReach, int negSize, int distToOrigin, (Position min, Position max) box)>(Comparer<(int, int, int, (Position, Position))>.Create((a, b) => a.CompareTo(b)))
        {
            (-nanobots.Length, -2 * boxSize, 3 * boxSize, initialBox)
        };

        while (workHeap.Count > 0)
        {
            var current = workHeap.Min;
            workHeap.Remove(current);

            var (_, negSize, distToOrig, box) = current;
            if (negSize == -1)
            {
                return distToOrig.ToString();
            }

            var newSize = negSize / -2;
            foreach (var oct in new[] { (0, 0, 0), (0, 0, 1), (0, 1, 0), (0, 1, 1), (1, 0, 0), (1, 0, 1), (1, 1, 0), (1, 1, 1) })
            {
                var newMin = new Position(box.min.X + newSize * oct.Item1, box.min.Y + newSize * oct.Item2, box.min.Z + newSize * oct.Item3);
                var newMax = new Position(newMin.X + newSize, newMin.Y + newSize, newMin.Z + newSize);
                var newBox = (newMin, newMax);
                var newReach = IntersectCount(newBox, nanobots);
                workHeap.Add((-newReach, -newSize, newMin.ManhattanDistanceFromOrigin(), newBox));
            }
        }

        return "";
    }

    private record Position(int X, int Y, int Z) : IComparable<Position>
    {
        public int CompareTo(Position? other)
        {
            if (other == null)
            {
                return 1;
            }

            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0)
            {
                return xComparison;
            }

            var yComparison = Y.CompareTo(other.Y);
            return yComparison != 0 ? yComparison : Z.CompareTo(other.Z);
        }

        public int ManhattanDistance(Position other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public int ManhattanDistanceFromOrigin()
        {
            return ManhattanDistance(new Position(0, 0, 0));
        }
    }

    private record Nanobot(Position Position, int R)
    {
        public int Strength(Nanobot[] nanobots)
        {
            return nanobots.Count(n => ManhattanDistance(n) <= R);
        }

        private int ManhattanDistance(Nanobot other)
        {
            return Position.ManhattanDistance(other.Position);
        }

        public bool DoesIntersect((Position min, Position max) box)
        {
            var d = 0;
            for (var i = 0; i < 3; i++)
            {
                var boxLow = i switch
                {
                    0 => box.min.X,
                    1 => box.min.Y,
                    _ => box.min.Z
                };
                var boxHigh = i switch
                {
                    0 => box.max.X - 1,
                    1 => box.max.Y - 1,
                    _ => box.max.Z - 1
                };
                var botCoord = i switch
                {
                    0 => Position.X,
                    1 => Position.Y,
                    _ => Position.Z
                };
                d += Math.Abs(botCoord - boxLow) + Math.Abs(botCoord - boxHigh);
                d -= boxHigh - boxLow;
            }
            d /= 2;
            return d <= R;
        }
    }

    private static int IntersectCount((Position min, Position max) box, Nanobot[] bots)
    {
        return bots.Count(x => x.DoesIntersect(box));
    }
}