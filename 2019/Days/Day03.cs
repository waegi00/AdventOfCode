using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(l => l.Split(','))
            .ToArray();

        var seen = Enumerable.Range(0, input.Length)
            .Select(_ => new HashSet<(int x, int y)>())
            .ToArray();

        for (var i = 0; i < input.Length; i++)
        {
            var (x, y) = (0, 0);

            foreach (var movement in input[i])
            {
                var ((dx, dy), d) = Move(movement);

                for (var j = 0; j < d; j++)
                {
                    x += dx;
                    y += dy;
                    seen[i].Add((x, y));
                }
            }
        }

        return seen[0]
            .Intersect(seen[1])
            .Min(x => Math.Abs(x.x) + Math.Abs(x.y))
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(l => l.Split(','))
            .ToArray();

        var seen = Enumerable.Range(0, input.Length)
            .Select(_ => new Dictionary<(int x, int y), int>())
            .ToArray();

        for (var i = 0; i < input.Length; i++)
        {
            var (x, y) = (0, 0);
            var l = 0;

            foreach (var movement in input[i])
            {
                var ((dx, dy), d) = Move(movement);

                for (var j = 0; j < d; j++)
                {
                    x += dx;
                    y += dy;
                    l++;
                    seen[i].TryAdd((x, y), l);
                }
            }
        }

        return seen[0].Keys
            .Intersect(seen[1].Keys)
            .Min(x => seen[0][x] + seen[1][x])
            .ToString();
    }

    private static ((int, int), int) Move(string str)
    {
        return (str[0] switch
        {
            'R' => (1, 0),
            'L' => (-1, 0),
            'U' => (0, 1),
            'D' => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(str))
        }, int.Parse(str[1..]));
    }
}