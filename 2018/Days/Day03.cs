using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToArray();

        const int size = 1000;
        var grid = new int[size][];
        foreach (var i in Enumerable.Range(0, size))
        {
            grid[i] = new int[size];
        }

        foreach (var line in input)
        {
            var pos = line[2][..^1].Split(',').Select(int.Parse).ToArray();
            var area = line[^1].Split('x').Select(int.Parse).ToArray();
            grid.SetValues(pos[0], pos[1], pos[0] + area[0] - 1, pos[1] + area[1] - 1, v => v + 1);
        }

        return grid.Sum(x => x.Count(y => y > 1)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToArray();

        const int size = 1000;
        var grid = new int[size][];
        foreach (var i in Enumerable.Range(0, size))
        {
            grid[i] = new int[size];
        }

        foreach (var line in input)
        {
            var pos = line[2][..^1].Split(',').Select(int.Parse).ToArray();
            var area = line[^1].Split('x').Select(int.Parse).ToArray();
            grid.SetValues(pos[0], pos[1], pos[0] + area[0] - 1, pos[1] + area[1] - 1, v => v + 1);
        }

        foreach (var line in input)
        {
            var id = int.Parse(line[0][1..]);
            var pos = line[2][..^1].Split(',').Select(int.Parse).ToArray();
            var area = line[^1].Split('x').Select(int.Parse).ToArray();

            var failed = false;
            for (var i = pos[0]; i < pos[0] + area[0] && !failed; i++)
            {
                for (var j = pos[1]; j < pos[1] + area[1] && !failed; j++)
                {
                    if (grid[i][j] != 1)
                    {
                        failed = true;
                    }
                }
            }

            if (!failed)
            {
                return id.ToString();
            }
        }

        return "";
    }
}