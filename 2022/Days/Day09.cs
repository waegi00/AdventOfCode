using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        return CalculateTailMovements(GetSteps(this.InputToLines()))
            .Count
            .ToString();
    }

    public string SolveSecond()
    {
        return CalculateTailMovements(GetSteps(this.InputToLines()), 10)
            .Count
            .ToString();
    }

    private static HashSet<(int, int)> CalculateTailMovements((int, int)[] steps, int length = 2)
    {
        var rope = Enumerable.Repeat((0, 0), length).ToArray();
        var tailMovements = new HashSet<(int, int)> { rope.Last() };

        foreach (var (x, y) in steps)
        {
            rope[0] = (rope[0].Item1 + x, rope[0].Item2 + y);
            for (var i = 1; i < rope.Length; i++)
            {
                rope[i] = KeepUp(rope[i - 1], rope[i]);
            }
            tailMovements.Add(rope.Last());
        }

        return tailMovements;
    }

    private static (int, int) KeepUp((int, int) lead, (int, int) follow)
    {
        if (Touching(lead, follow))
        {
            return follow;
        }

        var xDiff = lead.Item1 - follow.Item1;
        var yDiff = lead.Item2 - follow.Item2;

        if (xDiff == 0 || Math.Abs(xDiff) < Math.Abs(yDiff))
        {
            return (lead.Item1, lead.Item2 - (yDiff > 0 ? 1 : -1));
        }

        if (yDiff == 0 || Math.Abs(xDiff) > Math.Abs(yDiff))
        {
            return (lead.Item1 - (xDiff > 0 ? 1 : -1), lead.Item2);
        }

        return (lead.Item1 - (xDiff > 0 ? 1 : -1), lead.Item2 - (yDiff > 0 ? 1 : -1));
    }

    private static bool Touching((int, int) p1, (int, int) p2)
    {
        return Math.Abs(p1.Item1 - p2.Item1) <= 1 && Math.Abs(p1.Item2 - p2.Item2) <= 1;
    }

    private static (int, int)[] GetSteps(string[] lines)
    {
        var steps = new List<(int, int)>();
        foreach (var line in lines)
        {
            steps.AddRange(ParseStep(line));
        }

        return steps.ToArray();
    }

    private static (int, int)[] ParseStep(string step)
    {
        return Enumerable.Repeat(Direction(step[0]), int.Parse(step[2..])).ToArray();
    }

    private static (int, int) Direction(char dir)
    {
        return dir switch
        {
            'R' => (1, 0),
            'L' => (-1, 0),
            'U' => (0, 1),
            'D' => (0, -1),
            _ => (0, 0)
        };
    }
}