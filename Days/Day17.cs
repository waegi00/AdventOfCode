using System.Diagnostics.CodeAnalysis;
using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day17.txt");

        var points = new List<Point> { new(0, 0, 0, Direction.None, 0) }; // First field does not add to sum

        List<Point> newPoints;
        do
        {
            newPoints = new List<Point>();
            foreach (var point in points.Where(x => !x.Done).ToList())
            {
                if (point.X > 0 && point is not { Times: 3, From: Direction.Bottom } &&
                    point is not { From: Direction.Top })
                {
                    var times = point.From == Direction.Bottom ? point.Times + 1 : 1;
                    var newPoint = point with
                    {
                        X = point.X - 1,
                        Sum = point.Sum + int.Parse(input[point.X - 1][point.Y].ToString()),
                        From = Direction.Bottom,
                        Times = times,
                    };
                    newPoints.Add(newPoint);
                }

                if (point.Y > 0 && point is not { Times: 3, From: Direction.Right } &&
                    point is not { From: Direction.Left })
                {
                    var times = point.From == Direction.Right ? point.Times + 1 : 1;
                    var newPoint = point with
                    {
                        Y = point.Y - 1,
                        Sum = point.Sum + int.Parse(input[point.X][point.Y - 1].ToString()),
                        From = Direction.Right,
                        Times = times
                    };
                    newPoints.Add(newPoint);
                }

                if (point.X < input.Length - 1 && point is not { Times: 3, From: Direction.Top } &&
                    point is not { From: Direction.Bottom })
                {
                    var times = point.From == Direction.Top ? point.Times + 1 : 1;
                    var newPoint = point with
                    {
                        X = point.X + 1,
                        Sum = point.Sum + int.Parse(input[point.X + 1][point.Y].ToString()),
                        From = Direction.Top,
                        Times = times
                    };
                    newPoints.Add(newPoint);
                }

                if (point.Y < input[0].Length - 1 && point is not { Times: 3, From: Direction.Left } &&
                    point is not { From: Direction.Right })
                {
                    var times = point.From == Direction.Left ? point.Times + 1 : 1;
                    var newPoint = point with
                    {
                        Y = point.Y + 1,
                        Sum = point.Sum + int.Parse(input[point.X][point.Y + 1].ToString()),
                        From = Direction.Left,
                        Times = times
                    };
                    newPoints.Add(newPoint);
                }

            }

            points = points.Select(x => x with { Done = true }).ToList();

            points.AddRange(newPoints);
            points = points.OrderBy(x => x.Sum).DistinctBy(x => new { x.X, x.Y, x.From, x.Times }).ToList();
        } while (newPoints.Any());

        return points.Where(x => x.X == input.Length - 1 && x.Y == input[0].Length - 1).Min(x => x.Sum).ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day17.txt");

        var points = new List<Point> { new(0, 0, 0, Direction.None, 0) }; // First field does not add to sum

        List<Point> newPoints;
        do
        {
            newPoints = new List<Point>();
            foreach (var point in points.Where(x => !x.Done).ToList())
            {
                if (point is { From: Direction.Bottom, Times: < 4 })
                {
                    var newPoint = point with
                    {
                        X = point.X - 1,
                        Sum = point.Sum + int.Parse(input[point.X - 1][point.Y].ToString()),
                        From = Direction.Bottom,
                        Times = point.Times + 1
                    };
                    newPoints.Add(newPoint);
                }
                else if (point is { From: Direction.Left, Times: < 4 })
                {
                    var newPoint = point with
                    {
                        Y = point.Y + 1,
                        Sum = point.Sum + int.Parse(input[point.X][point.Y + 1].ToString()),
                        From = Direction.Left,
                        Times = point.Times + 1
                    };
                    newPoints.Add(newPoint);
                }
                else if (point is { From: Direction.Right, Times: < 4 })
                {
                    var newPoint = point with
                    {
                        Y = point.Y - 1,
                        Sum = point.Sum + int.Parse(input[point.X][point.Y - 1].ToString()),
                        From = Direction.Right,
                        Times = point.Times + 1
                    };
                    newPoints.Add(newPoint);
                }
                else if (point is { From: Direction.Top, Times: < 4 })
                {
                    var newPoint = point with
                    {
                        X = point.X + 1,
                        Sum = point.Sum + int.Parse(input[point.X + 1][point.Y].ToString()),
                        From = Direction.Top,
                        Times = point.Times + 1
                    };
                    newPoints.Add(newPoint);
                }
                else
                {
                    if ((point is { From: Direction.Bottom, X: > 0 } || point.X > 3) && point.From != Direction.Top && point is not { From: Direction.Bottom, Times: 10 })
                    {
                        var newPoint = point with
                        {
                            X = point.X - 1,
                            Sum = point.Sum + int.Parse(input[point.X - 1][point.Y].ToString()),
                            From = Direction.Bottom,
                            Times = point.From == Direction.Bottom ? point.Times + 1 : 1,
                        };
                        newPoints.Add(newPoint);
                    }
                    if ((point is { From: Direction.Right, Y: > 0 } || point.Y > 3) && point.From != Direction.Left && point is not { From: Direction.Right, Times: 10 })
                    {
                        var newPoint = point with
                        {
                            Y = point.Y - 1,
                            Sum = point.Sum + int.Parse(input[point.X][point.Y - 1].ToString()),
                            From = Direction.Right,
                            Times = point.From == Direction.Right ? point.Times + 1 : 1,
                        };
                        newPoints.Add(newPoint);
                    }
                    if ((point.From == Direction.Top && point.X < input.Length - 1 || point.X < input.Length - 4) && point.From != Direction.Bottom && point is not { From: Direction.Top, Times: 10 })
                    {
                        var newPoint = point with
                        {
                            X = point.X + 1,
                            Sum = point.Sum + int.Parse(input[point.X + 1][point.Y].ToString()),
                            From = Direction.Top,
                            Times = point.From == Direction.Top ? point.Times + 1 : 1,
                        };
                        newPoints.Add(newPoint);
                    }
                    if ((point.From == Direction.Left && point.Y < input[0].Length - 1 || point.Y < input[0].Length - 4) && point.From != Direction.Right && point is not { From: Direction.Left, Times: 10 })
                    {
                        var newPoint = point with
                        {
                            Y = point.Y + 1,
                            Sum = point.Sum + int.Parse(input[point.X][point.Y + 1].ToString()),
                            From = Direction.Left,
                            Times = point.From == Direction.Left ? point.Times + 1 : 1,
                        };
                        newPoints.Add(newPoint);
                    }
                }
            }

            points = points.Select(x => x with { Done = true }).ToList();

            points.AddRange(newPoints);
            points = points.OrderBy(x => x.Sum).DistinctBy(x => new { x.X, x.Y, x.From, x.Times }).ToList();
        } while (newPoints.Any());

        return points.Where(x => x.X == input.Length - 1 && x.Y == input[0].Length - 1).Min(x => x.Sum).ToString();
    }

    private record Point(int X, int Y, int Sum, Direction From, int Times, bool Done = false);

    private enum Direction { None, Bottom, Left, Top, Right }
}