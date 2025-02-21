using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        return new Game(input, 3).RunGame(false)!.Value.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var attackPower = 4;
        while (true)
        {
            var outcome = new Game(input, attackPower).RunGame(true);
            if (outcome.HasValue)
            {
                return outcome.Value.ToString();
            }

            attackPower++;
        }
    }
}
public class Game
{
    private static readonly (int dx, int dy)[] NeighbourDirections = [(0, -1), (-1, 0), (1, 0), (0, 1)];

    private readonly string[] _map;

    private List<Unit> _units = [];

    public Game(string[] initialMap, int elfAttackPower)
    {
        for (var y = 0; y < initialMap.Length; y++)
        {
            for (var x = 0; x < initialMap[y].Length; x++)
            {
                switch (initialMap[y][x])
                {
                    case 'G':
                        _units.Add(new Unit { X = x, Y = y, IsGoblin = true, Health = 200, AttackPower = 3 });
                        break;
                    case 'E':
                        _units.Add(new Unit { X = x, Y = y, IsGoblin = false, Health = 200, AttackPower = elfAttackPower });
                        break;
                }
            }
        }

        _map = initialMap.Select(l => l.Replace('G', '.').Replace('E', '.')).ToArray();
    }

    public int? RunGame(bool failOnElfDeath)
    {
        var rounds = 0;
        while (true)
        {
            _units = _units.OrderBy(u => u.Y).ThenBy(u => u.X).ToList();
            for (var i = 0; i < _units.Count; i++)
            {
                var u = _units[i];
                var targets = _units.Where(t => t.IsGoblin != u.IsGoblin).ToList();
                if (targets.Count == 0)
                {
                    return rounds * _units.Sum(ru => ru.Health);
                }

                if (!targets.Any(t => IsAdjacent(u, t)))
                {
                    TryMove(u, targets);
                }

                var bestAdjacent =
                    targets
                    .Where(t => IsAdjacent(u, t))
                    .OrderBy(t => t.Health)
                    .ThenBy(t => t.Y)
                    .ThenBy(t => t.X)
                    .FirstOrDefault();

                if (bestAdjacent == null)
                {
                    continue;
                }

                bestAdjacent.Health -= u.AttackPower;
                if (bestAdjacent.Health > 0)
                {
                    continue;
                }

                if (failOnElfDeath && !bestAdjacent.IsGoblin)
                {
                    return null;
                }

                var index = _units.IndexOf(bestAdjacent);
                _units.RemoveAt(index);
                if (index < i)
                {
                    i--;
                }
            }

            rounds++;
        }
    }

    private void TryMove(Unit u, List<Unit> targets)
    {
        var inRange = new HashSet<(int x, int y)>();
        foreach (var target in targets)
        {
            foreach (var (dx, dy) in NeighbourDirections)
            {
                var (nx, ny) = (target.X + dx, target.Y + dy);
                if (IsOpen(nx, ny))
                {
                    inRange.Add((nx, ny));
                }
            }
        }

        var queue = new Queue<(int x, int y)>();
        var seen = new Dictionary<(int x, int y), (int px, int py)>();
        queue.Enqueue((u.X, u.Y));
        seen.Add((u.X, u.Y), (-1, -1));
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            foreach (var (dx, dy) in NeighbourDirections)
            {
                (int x, int y) nei = (x + dx, y + dy);
                if (seen.ContainsKey(nei) || !IsOpen(nei.x, nei.y))
                {
                    continue;
                }

                queue.Enqueue(nei);
                seen.Add(nei, (x, y));
            }
        }

        List<(int tx, int ty, List<(int x, int y)> path)> paths =
            inRange
            .Select(t => (t.x, t.y, path: GetPath(t.x, t.y, seen, u)))
            .Where(t => t.path.Count != 0)
            .OrderBy(t => t.path.Count)
            .ThenBy(t => t.y)
            .ThenBy(t => t.x)
            .ToList();

        var bestPath = paths.FirstOrDefault().path;
        if (bestPath != null)
        {
            (u.X, u.Y) = bestPath[0];
        }
    }

    private static List<(int x, int y)> GetPath(int destX, int destY, Dictionary<(int x, int y), (int px, int py)>? seen, Unit u)
    {
        if (seen == null || !seen.ContainsKey((destX, destY)))
        {
            return [];
        }
        var path = new List<(int x, int y)>();
        var (x, y) = (destX, destY);
        while (x != u.X || y != u.Y)
        {
            path.Add((x, y));
            (x, y) = seen[(x, y)];
        }

        path.Reverse();
        return path;
    }

    private bool IsOpen(int x, int y) => _map[y][x] == '.' && _units.All(u => u.X != x || u.Y != y);

    private static bool IsAdjacent(Unit u1, Unit u2) => Math.Abs(u1.X - u2.X) + Math.Abs(u1.Y - u2.Y) == 1;

    private class Unit
    {
        public int X, Y;
        public bool IsGoblin;
        public int Health = 200;
        public int AttackPower;
    }
}