using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        return Solve().ToString();
    }

    public string SolveSecond()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPart2 = false)
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" -> "))
            .Select(x => x.Select(y => y.Split(',').Select(int.Parse).ToArray()).ToArray())
            .ToArray();

        var sand = (x: 500, y: 0);
        var grid = new HashSet<(int, int)>();
        foreach (var line in input)
        {
            var x = line[0][0];
            var y = line[0][1];

            for (var i = 1; i < line.Length; i++)
            {
                var dx = Math.Sign(line[i][0] - x);
                var dy = Math.Sign(line[i][1] - y);

                while (x != line[i][0] || y != line[i][1])
                {
                    grid.Add((x, y));
                    x += dx;
                    y += dy;
                }

                grid.Add((x, y));
            }
        }

        var maxY = grid.Max(x => x.Item2) + 1;
        var sandUnits = 0;

        while (true)
        {
            var curr = sand;

            while (true)
            {
                if (curr.y == maxY)
                {
                    if (!isPart2)
                    {
                        return sandUnits;
                    }
                    break;
                }

                if (!grid.Contains((curr.x, curr.y + 1)))
                {
                    curr.y += 1;
                }
                else if (!grid.Contains((curr.x - 1, curr.y + 1)))
                {
                    curr.x -= 1;
                    curr.y += 1;
                }
                else if (!grid.Contains((curr.x + 1, curr.y + 1)))
                {
                    curr.x += 1;
                    curr.y += 1;
                }
                else
                {
                    break;
                }
            }

            grid.Add(curr);
            sandUnits++;

            if (curr == sand)
            {
                return sandUnits;
            }
        }
    }
}