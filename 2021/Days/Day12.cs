using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day12 : IRiddle
{
    private static readonly Dictionary<string, List<string>> neighbours = new();

    public string SolveFirst()
    {
        neighbours.Clear();
        
        foreach (var line in this.InputToLines())
        {
            var parts = line.Split('-');
            neighbours.TryAdd(parts[0], []);
            neighbours.TryAdd(parts[1], []);
            neighbours[parts[0]].Add(parts[1]);
            neighbours[parts[1]].Add(parts[0]);
        }

        return Count([], "start").ToString();
    }

    public string SolveSecond()
    {
        neighbours.Clear();

        foreach (var line in this.InputToLines())
        {
            var parts = line.Split('-');
            neighbours.TryAdd(parts[0], []);
            neighbours.TryAdd(parts[1], []);
            neighbours[parts[0]].Add(parts[1]);
            neighbours[parts[1]].Add(parts[0]);
        }

        return Count([], "start", true).ToString();
    }

    private static int Count(HashSet<string> seen, string cave, bool isPart2 = false)
    {
        if (cave == "end")
        {
            return 1;
        }

        if (seen.Contains(cave))
        {
            if (cave == "start")
            {
                return 0;
            }

            if (char.IsLower(cave[0]))
            {
                if (!isPart2)
                {
                    return 0;
                }
                isPart2 = false;
            }
        }

        var newSeen = new HashSet<string>(seen) { cave };
        return neighbours[cave].Sum(n => Count(newSeen, n, isPart2));
    }
}