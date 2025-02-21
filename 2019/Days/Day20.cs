using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Drawing;
using AdventOfCode.Library.Array;

namespace AdventOfCode._2019.Days;

public class Day20 : IRiddle
{
    private readonly string[] Input;
    private readonly int mapHeight;
    private readonly int mapWidth;
    private char[,]? map;
    private readonly DefaultDictionary<Point, char> PortalLetters = new();
    private readonly Dictionary<Point, string> PointToGate = new();
    private readonly Dictionary<string, List<Point>> GateToPoints = new();

    private readonly DefaultDictionary<Point, bool> InsideGates = new();

    public Day20()
    {
        Input = this.InputToLines();
        mapHeight = Input.Length;
        mapWidth = Input[0].Length;
    }

    public string SolveFirst()
    {
        ParseMap();
        ProcessGates();
        var ans = WalkMaze(new MazeState { Location = GateToPoints["AA"][0], Distance = 0, Level = 0 });
        return ans!.Distance.ToString();
    }

    public string SolveSecond()
    {
        var ans = WalkMaze(new MazeState { Location = GateToPoints["AA"][0], Distance = 0, Level = 0 }, true);
        return ans!.Distance.ToString();
    }

    private MazeState? WalkMaze(MazeState? state, bool recursive = false)
    {
        var ToProcess = new Queue<MazeState?>();
        var visited = new HashSet<(Point, int)>();

        ToProcess.Enqueue(state);
        MazeState? current;
        while (ToProcess.Count > 0)
        {
            current = ToProcess.Dequeue()!;
            visited.Add((current.Location, current.Level));

            if (PointToGate.TryGetValue(current.Location, out var value))
            {
                if (PointToGate[current.Location].Equals("ZZ") && current.Level == 0)
                {
                    return current;
                }

                var otherGatePoint = GateToPoints[value].FirstOrDefault(p => p.Equals(current.Location) == false);
                if (otherGatePoint.X != 0 && otherGatePoint.Y != 0)
                {
                    (Point, int) otherSide = (otherGatePoint, current.Level);
                    if (recursive)
                    {
                        otherSide.Item2 += (InsideGates[current.Location] ? 1 : -1);
                    }

                    if (visited.Contains(otherSide) == false)
                    {
                        if (GateOpen(current, otherGatePoint, recursive))
                        {
                            var nextState = new MazeState { Location = otherGatePoint, Distance = current.Distance + 1, Level = otherSide.Item2 };
                            nextState.Gates.AddRange(current.Gates);
                            nextState.Gates.Add((PointToGate[current.Location], otherSide.Item2));
                            ToProcess.Enqueue(nextState);
                            continue;
                        }
                    }
                }
            }
            var nextPoints = Around(current.Location, 0, 0, mapWidth - 1, mapHeight - 1)
                .Where(p => map![p.Y, p.X] == '.' && visited.Contains((p, current.Level)) == false)
                .ToList();

            foreach (var p in nextPoints)
            {
                ToProcess.Enqueue(new MazeState { Location = p, Distance = current.Distance + 1, Level = current.Level, Gates = current.Gates });
            }
        }

        return null;
    }

    private bool GateOpen(MazeState state, Point to, bool recursive)
    {
        if (!recursive)
        {
            return true;
        }

        var otherGateName = PointToGate[to];

        if (otherGateName is "AA" or "ZZ")
        {
            return state.Level == 0;
        }

        return state.Level != 0 || InsideGates[state.Location];
    }

    private void ParseMap()
    {
        map = new char[mapHeight, mapWidth];

        for (var y = 0; y < mapHeight; y++)
        {
            for (var x = 0; x < mapWidth; x++)
            {
                var curChar = Input[y][x];
                switch (curChar)
                {
                    case ' ':
                        map[y, x] = '#';
                        break;
                    case >= 'A' and <= 'Z':
                        {
                            if (Around(new Point(x, y)).Select(p => PortalLetters.ContainsKey(p) ? 1 : 0).Sum() == 0)
                            {
                                PortalLetters.Add(new Point(x, y), curChar);
                            }

                            break;
                        }
                }

                map[y, x] = curChar;
            }
        }
    }

    private void ProcessGates()
    {
        foreach (var kvp in PortalLetters)
        {
            var otherPoint = Around(kvp.Key, 0, 0, mapWidth - 1, mapHeight - 1)
                .First(p => map![p.Y, p.X] >= 'A' && map[p.Y, p.X] <= 'Z');

            var diff = Subtract(otherPoint, kvp.Key);

            var entrance = Add(otherPoint, diff);
            if (entrance.X >= mapWidth || entrance.Y >= mapHeight || map![entrance.Y, entrance.X] != '.')
            {
                entrance = Subtract(kvp.Key, diff);
            }

            var gateName = new string([map![kvp.Key.Y, kvp.Key.X], map[otherPoint.Y, otherPoint.X]]);
            PointToGate.Add(entrance, gateName);

            if (GateToPoints.ContainsKey(gateName) == false)
            {
                GateToPoints.Add(gateName, []);
            }

            GateToPoints[gateName].Add(entrance);

            if (entrance.Y > 3 && entrance.Y < mapHeight - 3 && entrance.X > 3 && entrance.X < mapWidth - 3)
            {
                InsideGates.Add(entrance, true);
            }

        }
    }

    private class MazeState
    {
        public Point Location { get; init; }
        public int Distance { get; init; }
        public int Level { get; init; }
        public List<(string, int)> Gates = [];

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == this.GetType() == false)
            {
                return false;
            }

            var cast = (MazeState)obj;
            return cast.Location.Equals(this.Location) && cast.Distance == Distance && cast.Level == Level;
        }

        public override int GetHashCode() => HashCode.Combine(Location, Distance, Level);
    }

    private static Point Add(Point p, Point p2)
    {
        return new Point(p.X + p2.X, p.Y + p2.Y);
    }

    private static Point Subtract(Point p, Point p2)
    {
        return new Point(p.X - p2.X, p.Y - p2.Y);
    }

    private static IEnumerable<Point> Around(Point p)
    {
        yield return p with { X = p.X + 1 };
        yield return p with { X = p.X - 1 };
        yield return p with { Y = p.Y + 1 };
        yield return p with { Y = p.Y - 1 };
    }

    private static IEnumerable<Point> Around(Point p, int minX, int minY, int maxX, int maxY)
    {
        if (p.X + 1 >= minX && p.X + 1 <= maxX)
        {
            yield return p with { X = p.X + 1 };
        }
        if (p.X - 1 >= minX && p.X - 1 <= maxX)
        {
            yield return p with { X = p.X - 1 };
        }
        if (p.Y + 1 >= minY && p.Y + 1 <= maxY)
        {
            yield return p with { Y = p.Y + 1 };
        }
        if (p.Y - 1 >= minY && p.Y - 1 <= maxY)
        {
            yield return p with { Y = p.Y - 1 };
        }
    }
}
