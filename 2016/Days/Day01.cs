using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(", ")
            .Select(line => (line[0], int.Parse(line[1..])))
            .ToList();

        var (dx, dy) = (0, -1);
        var (x, y) = (0, 0);

        foreach (var (rot, amount) in input)
        {
            (dx, dy) = rot == 'R' ? (-dy, dx) : (dy, -dx);
            (x, y) = (x + dx * amount, y + dy * amount);
        }

        return (Math.Abs(x) + Math.Abs(y)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(", ")
            .Select(line => (line[0], int.Parse(line[1..])))
            .ToList();

        var (dx, dy) = (0, -1);
        var (x, y) = (0, 0);
        var visited = new HashSet<(int, int)> { (0, 0) };

        foreach (var (rot, amount) in input)
        {
            (dx, dy) = rot == 'R' ? (-dy, dx) : (dy, -dx);
            for (var i = 1; i <= amount; i++)
            {
                (x, y) = (x + dx, y + dy);
                if (!visited.Add((x, y)))
                {
                    return (Math.Abs(x) + Math.Abs(y)).ToString();
                }
            }
        }

        return "";
    }
}