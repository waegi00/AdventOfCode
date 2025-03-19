using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public partial class Day15 : IRiddle
{
    public string SolveFirst()
    {
        const long bounds = 2000000;

        var data = this.InputToLines()
            .Select(line => NumberRegex().Matches(line)
                .Select(m => long.Parse(m.Value)).ToArray())
            .Select(arr => (x: arr[0], y: arr[1], d: Math.Abs(arr[0] - arr[2]) + Math.Abs(arr[1] - arr[3])))
            .ToArray();

        return (data.Max(t => t.x - Math.Abs(bounds - t.y) + t.d) -
                data.Min(t => t.x + Math.Abs(bounds - t.y) - t.d))
            .ToString();
    }

    public string SolveSecond()
    {
        const long bounds = 4000000;

        var data = this.InputToLines()
            .Select(line => NumberRegex().Matches(line)
                .Select(m => long.Parse(m.Value)).ToArray())
            .Select(arr => (x: arr[0], y: arr[1], d: Math.Abs(arr[0] - arr[2]) + Math.Abs(arr[1] - arr[3])))
            .ToArray();

        foreach (var (X, Y) in data.SelectMany(a => data.Select(b => DistressSignal(a.x, a.y, a.d, b.x, b.y, b.d))))
        {
            if (X is > 0 and < bounds && Y is > 0 and < bounds && data.All(t => Distance(X, Y, t.x, t.y) > t.d))
            {
                return (bounds * X + Y).ToString();
            }
        }

        return "";
    }

    private static long Distance(long x, long y, long p, long q) => Math.Abs(x - p) + Math.Abs(y - q);

    private static (long, long) DistressSignal(long x, long y, long d, long p, long q, long r)
    {
        return (
            (p + q + r + x - y - d) / 2,
            (p + q + r - x + y + d) / 2 + 1
        );
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex NumberRegex();
}