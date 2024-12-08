using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2024.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().ToCharArray();

        var dict = new Dictionary<char, List<(int x, int y)>>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '.') continue;
                if (!dict.TryAdd(input[i][j], [(i, j)]))
                {
                    dict[input[i][j]].Add((i, j));
                }
            }
        }

        var positions = new HashSet<(int, int)>();

        foreach (var values in dict.Values)
        {
            foreach (var ((x1, y1), (x2, y2)) in values.Pairs())
            {
                var dx = x2 - x1;
                var dy = y2 - y1;

                if (input.IsValidPosition(x1 - dx, y1 - dy))
                {
                    positions.Add((x1 - dx, y1 - dy));
                }
                if (input.IsValidPosition(x2 + dx, y2 + dy))
                {
                    positions.Add((x2 + dx, y2 + dy));
                }
            }
        }

        return positions.Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();

        var dict = new Dictionary<char, List<(int x, int y)>>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '.') continue;
                if (!dict.TryAdd(input[i][j], [(i, j)]))
                {
                    dict[input[i][j]].Add((i, j));
                }
            }
        }

        var positions = new HashSet<(int, int)>();

        foreach (var values in dict.Values)
        {
            foreach (var ((x1, y1), (x2, y2)) in values.Pairs())
            {
                var dx = x2 - x1;
                var dy = y2 - y1;

                var up = true;
                var down = true;
                var n = 0;
                while (up || down)
                {
                    if (input.IsValidPosition(x1 + n * dx, y1 + n * dy))
                    {
                        positions.Add((x1 + n * dx, y1 + n * dy));
                    }
                    else
                    {
                        up = false;
                    }
                    if (input.IsValidPosition(x1 - n * dx, y1 - n * dy))
                    {
                        positions.Add((x1 - n * dx, y1 - n * dy));
                    }
                    else
                    {
                        down = false;
                    }
                    n++;
                }
            }
        }

        return positions.Count.ToString();
    }
}