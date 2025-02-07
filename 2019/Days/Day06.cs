using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(')'))
            .ToArray();

        var orbits = new Dictionary<string, HashSet<string>>();
        foreach (var orbit in input)
        {
            orbits.TryAdd(orbit[0], []);
            orbits[orbit[0]].Add(orbit[1]);
        }

        var parent = orbits.First(o
            => orbits.Values.All(v => !v.Contains(o.Key)))
            .Key;

        return OrbitCount(orbits, parent).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(')'))
            .ToArray();

        var orbits = new Dictionary<string, HashSet<string>>();
        var parent = new Dictionary<string, string>();
        foreach (var orbit in input)
        {
            orbits.TryAdd(orbit[0], []);
            orbits[orbit[0]].Add(orbit[1]);
            parent.Add(orbit[1], orbit[0]);
        }

        var seen = new Dictionary<string, int>();

        var start = orbits
            .First(o => o.Value.Contains("YOU"))
            .Key;

        var end = orbits
            .First(o => o.Value.Contains("SAN"))
            .Key;

        var i = 0;
        while (parent.TryGetValue(start, out var pStart) | parent.TryGetValue(end, out var pEnd))
        {
            if (pStart != null)
            {
                if (!seen.TryAdd(pStart, i + 1))
                {
                    return (seen[pStart] + i + 1).ToString();
                }
                start = pStart;
            }

            if (pEnd != null)
            {
                if (!seen.TryAdd(pEnd, i + 1))
                {
                    return (seen[pEnd] + i + 1).ToString();
                }
                end = pEnd;
            }

            i++;
        }

        return "";
    }

    private static int OrbitCount(Dictionary<string, HashSet<string>> orbits, string root, int depth = 0)
    {
        return depth +
               (orbits.TryGetValue(root, out var children)
                   ? children.Sum(x => OrbitCount(orbits, x, depth + 1))
                   : 0);
    }
}