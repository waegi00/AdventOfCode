using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public partial class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var grid = new bool[1000][];
        for (var i = 0; i < 1000; i++)
        {
            grid[i] = new bool[1000];
        }

        foreach (var line in input)
        {
            var numsRegex = MyRegex();

            var matches = numsRegex.Matches(line);
            var m1 = matches[0].Value.Split(',');
            var m2 = matches[1].Value.Split(',');

            var i1 = int.Parse(m1[0]);
            var j1 = int.Parse(m1[1]);
            var i2 = int.Parse(m2[0]);
            var j2 = int.Parse(m2[1]);

            switch (line[6])
            {
                case 'n':
                    grid.SetValues(i1, j1, i2, j2, _ => true);
                    break;
                case 'f':
                    grid.SetValues(i1, j1, i2, j2, _ => false);
                    break;
                default:
                    grid.SetValues(i1, j1, i2, j2, a => !a);
                    break;
            }
        }

        return grid.SelectMany(x => x).Sum(b => b ? 1 : 0).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var grid = new int[1000][];
        for (var i = 0; i < 1000; i++)
        {
            grid[i] = new int[1000];
        }

        foreach (var line in input)
        {
            var numsRegex = MyRegex();

            var matches = numsRegex.Matches(line);
            var m1 = matches[0].Value.Split(',');
            var m2 = matches[1].Value.Split(',');

            var i1 = int.Parse(m1[0]);
            var j1 = int.Parse(m1[1]);
            var i2 = int.Parse(m2[0]);
            var j2 = int.Parse(m2[1]);

            switch (line[6])
            {
                case 'n':
                    grid.SetValues(i1, j1, i2, j2, i => i + 1);
                    break;
                case 'f':
                    grid.SetValues(i1, j1, i2, j2, i => i - 1 > 0 ? i - 1 : 0);
                    break;
                default:
                    grid.SetValues(i1, j1, i2, j2, i => i + 2);
                    break;
            }
        }

        return grid.SelectMany(x => x).Sum().ToString();
    }

    [GeneratedRegex(@"\d+,\d+")]
    private static partial Regex MyRegex();
}