using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public partial class Day20 : IRiddle
{
    public string SolveFirst()
    {
        var particles = this.InputToLines()
            .Select(line => Vector3Regex()
                .Matches(line)
                .Select(match => new Vector3(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value)))
                .ToList())
                .Select(vs => new Particle(vs[0], vs[1], vs[2]))
            .ToList();

        return particles
            .Select((x, i) => (x, i))
            .OrderBy(x => x.x.a.Magnitude)
            .ThenBy(x => x.x.v.Magnitude)
            .ThenBy(x => x.x.x.Magnitude)
            .First().i
            .ToString();
    }

    public string SolveSecond()
    {
        var particles = this.InputToLines()
            .Select(line => Vector3Regex()
                .Matches(line)
                .Select(match => new Vector3(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value)))
                .ToList())
            .Select(vs => new Particle(vs[0], vs[1], vs[2]))
            .ToList();

        var c = particles.Count;
        var last = 0;
        var t = 0;

        while (true)
        {
            particles.ForEach(p => p.Move());

            particles = particles
                .GroupBy(p => p.x)
                .Where(g => g.Count() == 1)
                .SelectMany(g => g.ToList())
                .ToList();

            if (particles.Count != c)
            {
                last = t;
                c = particles.Count;
            }

            if (t - last > 1_000)
            {
                return particles.Count.ToString();
            }

            t++;
        }
    }

    [GeneratedRegex(@"<\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)\s*>")]
    private static partial Regex Vector3Regex();

    private record Vector3
    {
        private readonly int X;
        private readonly int Y;
        private readonly int Z;

        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
    }

    private record Particle(Vector3 x, Vector3 v, Vector3 a)
    {
        public Vector3 x { get; private set; } = x;
        public Vector3 v { get; private set; } = v;
        public readonly Vector3 a = a;

        public void Move()
        {
            v += a;
            x += v;
        }
    }
}