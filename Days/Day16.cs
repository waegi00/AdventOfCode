using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day16.txt");

        var matrix = new char[input.Length, input[0].Length];

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                matrix[i, j] = input[i][j];
            }
        }

        var energized = new List<Beam>();
        var beams = new List<Beam> { new Beam(0, 0, Direction.Rightward) };

        while (beams.Count != 0)
        {
            energized.AddRange(beams);
            var newBeams = new List<Beam>();
            foreach (var beam in beams)
            {
                var c = matrix[beam.X, beam.Y];

                switch (beam.Direction)
                {
                    case Direction.Rightward:
                        if ((c == '.' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y + 1, beam.Direction));
                        }
                        if ((c == '/' || c == '|') && beam.X > 0)
                        {
                            newBeams.Add(new Beam(beam.X - 1, beam.Y, Direction.Upward));
                        }
                        if ((c == '\\' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                        {
                            newBeams.Add(new Beam(beam.X + 1, beam.Y, Direction.Downward));
                        }
                        break;

                    case Direction.Leftward:
                        if ((c == '.' || c == '-') && beam.Y > 0)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y - 1, beam.Direction));
                        }
                        if ((c == '/' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                        {
                            newBeams.Add(new Beam(beam.X + 1, beam.Y, Direction.Downward));
                        }
                        if ((c == '\\' || c == '|') && beam.X > 0)
                        {
                            newBeams.Add(new Beam(beam.X - 1, beam.Y, Direction.Upward));
                        }
                        break;

                    case Direction.Upward:
                        if ((c == '.' || c == '|') && beam.X > 0)
                        {
                            newBeams.Add(new Beam(beam.X - 1, beam.Y, beam.Direction));
                        }
                        if ((c == '/' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y + 1, Direction.Rightward));
                        }
                        if ((c == '\\' || c == '-') && beam.Y > 0)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y - 1, Direction.Leftward));
                        }
                        break;

                    case Direction.Downward:
                        if ((c == '.' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                        {
                            newBeams.Add(new Beam(beam.X + 1, beam.Y, beam.Direction));
                        }
                        if ((c == '/' || c == '-') && beam.Y > 0)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y - 1, Direction.Leftward));
                        }
                        if ((c == '\\' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                        {
                            newBeams.Add(new Beam(beam.X, beam.Y + 1, Direction.Rightward));
                        }
                        break;
                }
            }

            beams = newBeams.Where(x => !energized.Contains(x)).ToList();
        }

        return energized.DistinctBy(x => new { x.X, x.Y }).Count().ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day16.txt");

        var max = -1;

        foreach (var dir in Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            for (var k = 0; k < input.Length; k++)
            {
                var matrix = new char[input.Length, input[0].Length];

                for (var i = 0; i < input.Length; i++)
                {
                    for (var j = 0; j < input[i].Length; j++)
                    {
                        matrix[i, j] = input[i][j];
                    }
                }

                var energized = new List<Beam>();
                var beams = new List<Beam>
                {
                    new Beam(
                        dir == Direction.Rightward || dir == Direction.Leftward ? k : dir == Direction.Upward ? matrix.GetLength(0) - 1 : 0,
                        dir == Direction.Downward || dir == Direction.Upward ? k : dir == Direction.Leftward ? matrix.GetLength(1) - 1 : 0,
                        dir)
                };

                while (beams.Count != 0)
                {
                    energized.AddRange(beams);
                    var newBeams = new List<Beam>();
                    foreach (var beam in beams)
                    {
                        var c = matrix[beam.X, beam.Y];

                        switch (beam.Direction)
                        {
                            case Direction.Rightward:
                                if ((c == '.' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y + 1, beam.Direction));
                                }
                                if ((c == '/' || c == '|') && beam.X > 0)
                                {
                                    newBeams.Add(new Beam(beam.X - 1, beam.Y, Direction.Upward));
                                }
                                if ((c == '\\' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X + 1, beam.Y, Direction.Downward));
                                }
                                break;

                            case Direction.Leftward:
                                if ((c == '.' || c == '-') && beam.Y > 0)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y - 1, beam.Direction));
                                }
                                if ((c == '/' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X + 1, beam.Y, Direction.Downward));
                                }
                                if ((c == '\\' || c == '|') && beam.X > 0)
                                {
                                    newBeams.Add(new Beam(beam.X - 1, beam.Y, Direction.Upward));
                                }
                                break;

                            case Direction.Upward:
                                if ((c == '.' || c == '|') && beam.X > 0)
                                {
                                    newBeams.Add(new Beam(beam.X - 1, beam.Y, beam.Direction));
                                }
                                if ((c == '/' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y + 1, Direction.Rightward));
                                }
                                if ((c == '\\' || c == '-') && beam.Y > 0)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y - 1, Direction.Leftward));
                                }
                                break;

                            case Direction.Downward:
                                if ((c == '.' || c == '|') && beam.X < matrix.GetLength(0) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X + 1, beam.Y, beam.Direction));
                                }
                                if ((c == '/' || c == '-') && beam.Y > 0)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y - 1, Direction.Leftward));
                                }
                                if ((c == '\\' || c == '-') && beam.Y < matrix.GetLength(1) - 1)
                                {
                                    newBeams.Add(new Beam(beam.X, beam.Y + 1, Direction.Rightward));
                                }
                                break;
                        }
                    }

                    beams = newBeams.Where(x => !energized.Contains(x)).ToList();
                }

                if (energized.DistinctBy(x => new { x.X, x.Y }).Count() > max)
                {
                    max = energized.DistinctBy(x => new { x.X, x.Y }).Count();
                }
            }
        }

        return max.ToString();
    }

    private record Beam(int X, int Y, Direction Direction);

    private enum Direction { Downward, Rightward, Upward, Leftward }
}