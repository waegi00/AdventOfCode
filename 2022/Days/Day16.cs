using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day16 : IRiddle
{

    public string SolveFirst()
    {
        var max = 0;
        var (valves, distances) = Parse();

        Backtrack(valves, distances, 30, "AA", 0, 0, valves.Count(v => v.Value is { Rate: > 0, IsOpen: false }), ref max, [], []);

        return max.ToString();
    }

    public string SolveSecond()
    {
        var max = 0;
        var (valves, distances) = Parse();
        
        var cache = new Dictionary<HashSet<string>, int>();
        Backtrack(valves, distances, 26, "AA", 0, 0, valves.Count(v => v.Value is { Rate: > 0, IsOpen: false }), ref max, [], cache);

        var pressures = cache.Keys
            .OrderByDescending(x => cache[x])
            .ToArray();

        foreach (var a in pressures)
        {
            foreach (var b in pressures)
            {
                if (cache[a] + cache[b] < max)
                {
                    return max.ToString();
                }

                if (!a.Overlaps(b))
                {
                    max = cache[a] + cache[b];
                }
            }
        }

        return "";
    }

    private (Dictionary<string, Valve> valves, Dictionary<(string, string), int> distances) Parse()
    {

        var valves = new Dictionary<string, Valve>();
        foreach (var line in this.InputToLines())
        {
            var splits = line.Split("; ");
            var firstPart = splits[0].Split(' ');
            valves.Add(firstPart[1], new Valve
            {
                Rate = int.Parse(firstPart[^1].Split('=')[1]),
                Connected = splits[1].Split([' ', ','], StringSplitOptions.RemoveEmptyEntries)[4..]
            });
        }

        var distances = new Dictionary<(string, string), int>();
        foreach (var i in valves.Keys)
        {
            foreach (var j in valves.Keys)
            {
                var q = new Queue<(string node, int l)>();
                q.Enqueue((i, 0));
                var visited = new HashSet<string>();

                while (q.Count > 0)
                {
                    var curr = q.Dequeue();

                    if (curr.node == j)
                    {
                        distances[(i, j)] = curr.l;
                        break;
                    }

                    foreach (var n in valves[curr.node].Connected)
                    {
                        if (visited.Add(n))
                        {
                            q.Enqueue((n, curr.l + 1));
                        }
                    }
                }
            }
        }

        return (valves, distances);
    }

    private static void Backtrack(
        Dictionary<string, Valve> valves,
        Dictionary<(string, string), int> distances,
        int time,
        string currentValve,
        int pressure,
        int totalPressure,
        int available,
        ref int max,
        List<string> currentPath,
        Dictionary<HashSet<string>, int> cache)
    {
        if (time == 0 || available == 0)
        {
            max = Math.Max(max, totalPressure + pressure * time);
            return;
        }

        cache.Add([.. currentPath], totalPressure + pressure * time);

        foreach (var n in valves.Keys)
        {
            if (n == currentValve || valves[n].IsOpen || valves[n].Rate == 0) continue;

            var timeToGo = distances[(n, currentValve)] + 1;
            if (timeToGo > time)
            {
                timeToGo = time;
            }
            valves[n].IsOpen = true;
            currentPath.Add(n);
            Backtrack(valves, distances, time - timeToGo, n, pressure + valves[n].Rate, totalPressure + pressure * timeToGo, available - 1, ref max, currentPath, cache);
            currentPath.RemoveAt(currentPath.Count - 1);
            valves[n].IsOpen = false;
        }
    }

    private class Valve
    {
        public int Rate { get; init; }
        public string[] Connected { get; init; } = [];
        public bool IsOpen { get; set; }
    }
}