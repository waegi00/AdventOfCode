using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .GroupBy(a => a)
            .ToDictionary(a => a.Key, a => a.Count());

        var n = input["n"];
        var ne = input["ne"];
        var se = input["se"];
        var s = input["s"];
        var sw = input["sw"];
        var nw = input["nw"];

        var (x, y, z) = (ne + se - nw - sw, n + nw - s - se, s + sw - n - ne);

        return ((Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',');

        var max = 0;

        var (x, y, z) = (0, 0, 0);

        foreach (var dir in input)
        {
            switch (dir)
            {
                case "n":
                    y++;
                    z--;
                    break;
                case "ne":
                    x++;
                    z--;
                    break;
                case "se":
                    x++;
                    y--;
                    break;
                case "s":
                    y--;
                    z++;
                    break;
                case "sw":
                    x--;
                    z++;
                    break;
                case "nw":
                    x--;
                    y++;
                    break;
            }

            max = Math.Max(max, (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2);
        }

        return max.ToString();
    }
}