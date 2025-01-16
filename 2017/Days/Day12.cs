using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" <-> "))
            .ToDictionary(line => line[0], line => line[1].Split(", "));

        var visited = new HashSet<string>();
        var toVisit = new Queue<string>();
        toVisit.Enqueue("0");

        while (toVisit.TryDequeue(out var curr))
        {
            if (!visited.Add(curr))
            {
                continue;
            }

            foreach (var node in input[curr])
            {
                toVisit.Enqueue(node);
            }
        }

        return visited.Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" <-> "))
            .ToDictionary(line => line[0], line => line[1].Split(", "));

        var groups = 0;
        while (input.Count > 0)
        {
            groups++;

            var visited = new HashSet<string>();
            var toVisit = new Queue<string>();
            toVisit.Enqueue(input.Keys.First());

            while (toVisit.TryDequeue(out var curr))
            {
                if (!visited.Add(curr))
                {
                    continue;
                }

                foreach (var node in input[curr])
                {
                    toVisit.Enqueue(node);
                }

                input.Remove(curr);
            }
        }

        return groups.ToString();
    }
}