using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public partial class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var numbers = NumberRegex()
            .Matches(this.InputToText())
            .Select(m => int.Parse(m.Value))
            .ToArray();

        return ((numbers[2] + 1) * numbers[2] / 2).ToString();
    }

    public string SolveSecond()
    {
        var numbers = NumberRegex()
            .Matches(this.InputToText())
            .Select(m => int.Parse(m.Value))
            .ToArray();

        return Process(numbers[0], numbers[1], numbers[2], numbers[3]).ToString();
    }

    private static int Process(int xMin, int xMax, int yMin, int yMax)
    {
        var total = 0;

        for (var vx = 0; vx <= Math.Max(xMax, 0); vx++)
        {
            for (var vy = yMin; vy <= Math.Abs(yMax) * 2; vy++)
            {
                if (Simulate(vx, vy, xMin, xMax, yMin, yMax))
                {
                    total++;
                }
            }
        }
        return total;
    }

    private static bool Simulate(int vx, int vy, int xMin, int xMax, int yMin, int yMax)
    {
        var x = 0;
        var y = 0;
        while (Math.Abs(x) <= Math.Max(Math.Abs(xMin), Math.Abs(xMax)) && y >= yMin)
        {
            if (xMin <= x && x <= xMax && yMin <= y && y <= yMax)
            {
                return true;
            }

            x += vx;
            y += vy;
            vx += new[] { -1, 0, 1 }[(vx < 0 ? 1 : 0) + (vx <= 0 ? 1 : 0)];
            vy -= 1;
        }
        return false;
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex NumberRegex();
}