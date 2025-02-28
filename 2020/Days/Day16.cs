using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2020.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n");

        var validRanges = input[0]
            .Split("\r\n")
            .Select(x => x.Split(':')[1]
                .Split(" or ")
                .Select(y => y.Split('-')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray())
            .ToArray();

        var valid = new HashSet<int>();

        foreach (var row in validRanges)
        {
            foreach (var pair in row)
            {
                for (var i = pair[0]; i <= pair[1]; i++)
                {
                    valid.Add(i);
                }
            }
        }

        return input[^1]
            .Split("\r\n")
            .Skip(1)
            .SelectMany(x => x.Split(',').Select(int.Parse))
            .Where(x => !valid.Contains(x))
            .Sum()
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n");

        var ranges = input[0]
            .Split("\r\n")
            .Select(x =>
            {
                var splits = x.Split(": ");
                return new Range(
                    splits[0],
                    splits[1].Split(" or ")
                        .Select(y => y.Split('-').Select(int.Parse).ToArray())
                        .Select(y => (y[0], y[1]))
                        .ToArray());
            }).ToArray();

        var nearby = input[^1]
            .Split("\r\n")
            .Skip(1)
            .Select(x => x.Split(',').Select(int.Parse).ToArray())
            .Where(x => x.All(y => ranges.Any(r => r.IsInRange(y))))
            .ToArray();

        var possible = Enumerable.Range(0, nearby[0].Length)
            .ToDictionary(x => x, _ => ranges.ToHashSet());

        foreach (var ticket in nearby)
        {
            foreach (var (x, i) in ticket.Select((x, i) => (x, i)))
            {
                foreach (var range in possible[i].Where(range => !range.IsInRange(x)))
                {
                    possible[i].Remove(range);
                }
            }
        }

        var result = new Dictionary<int, Range>();
        while (possible.Count > 0)
        {
            var (k, v) = possible.First(x => x.Value.Count == 1);
            var val = v.First();
            result.Add(k, val);

            possible.Remove(k);
            foreach (var kvp in possible)
            {
                possible[kvp.Key].Remove(val);
            }
        }

        return input[1]
            .Split("\r\n")[^1]
            .Split(',')
            .Select(long.Parse)
            .Where((_, i) => result[i].Name.StartsWith("departure"))
            .Product()
            .ToString();
    }

    private record Range(string Name, (int start, int end)[] Ranges)
    {
        public bool IsInRange(int n) => Ranges.Any(x => n.IsBetween(x.start, x.end));
    }
}