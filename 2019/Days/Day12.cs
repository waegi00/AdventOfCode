using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2019.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        const int steps = 1000;
        var input = this.InputToLines()
            .Select(x => x.Split(["<x=", ", y=", ", z=", ">"], StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToArray())
            .ToList();

        var moons = input
            .Select(x => new Moon(
                new Vector3(x[0], x[1], x[2]),
                new Vector3(0, 0, 0)))
            .ToList();

        for (var i = 0; i < steps; i++)
        {
            for (var m = 0; m < moons.Count; m++)
            {
                for (var o = 0; o < moons.Count; o++)
                {
                    if (o == m) continue;
                    moons[m] = moons[m].Accelerate(moons[o]);
                }
            }

            moons = moons.Select(x => x.Move()).ToList();
        }

        return moons.Sum(m => m.TotalEnergy()).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(["<x=", ", y=", ", z=", ">"], StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToArray())
            .ToList();

        var moons = input
            .Select(x => new Moon(
                new Vector3(x[0], x[1], x[2]),
                new Vector3(0, 0, 0)))
            .ToArray();

        var cycleX = OneDOrbits(moons.Select(m => m.position.X).ToList());
        var cycleY = OneDOrbits(moons.Select(m => m.position.Y).ToList());
        var cycleZ = OneDOrbits(moons.Select(m => m.position.Z).ToList());

        return (cycleX.LCM(cycleY.LCM(cycleZ))).ToString();
    }

    private static long OneDOrbits(List<int> positions)
    {
        var velocities = new List<int>(new int[positions.Count]);
        var firstState = positions.Concat(velocities).Aggregate((a, b) => a * 31 + b).GetHashCode();
        var steps = 0;

        while (true)
        {
            for (var idx1 = 0; idx1 < positions.Count; idx1++)
            {
                for (var idx2 = idx1 + 1; idx2 < positions.Count; idx2++)
                {
                    if (positions[idx1] < positions[idx2])
                    {
                        velocities[idx1] += 1;
                        velocities[idx2] -= 1;
                    }
                    else if (positions[idx1] > positions[idx2])
                    {
                        velocities[idx1] -= 1;
                        velocities[idx2] += 1;
                    }
                }
            }

            for (var x = 0; x < positions.Count; x++)
            {
                positions[x] += velocities[x];
            }

            steps++;

            var currentState = positions.Concat(velocities).Aggregate((a, b) => a * 31 + b).GetHashCode();
            if (firstState == currentState)
            {
                return steps;
            }
        }
    }

    private record Moon(Vector3 position, Vector3 velocity)
    {
        public Moon Move() => this with { position = position.Add(velocity) };

        public Moon Accelerate(Moon other) =>
            this with
            {
                velocity = velocity.Add(new Vector3(
                    Math.Sign(other.position.X.CompareTo(position.X)),
                    Math.Sign(other.position.Y.CompareTo(position.Y)),
                    Math.Sign(other.position.Z.CompareTo(position.Z))))
            };

        public int TotalEnergy() => PotentialEnergy() * KineticEnergy();
        private int PotentialEnergy() => position.Magnitude();
        private int KineticEnergy() => velocity.Magnitude();
    }

    private record Vector3(int X, int Y, int Z)
    {
        public Vector3 Add(Vector3 other) => new Vector3(X + other.X, Y + other.Y, Z + other.Z);
        public int Magnitude() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
    }
}