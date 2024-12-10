using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2024.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().ToCharArray();

        var dict = new Dictionary<int, List<(int x, int y)>>();

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var n = input[i][j] - '0';
                if (!dict.TryAdd(n, [(i, j)]))
                {
                    dict[n].Add((i, j));
                }
            }
        }

        var matching = new Dictionary<(int x, int y), List<(int x, int y)>>();
        for (var n = 0; n < 9; n++)
        {
            foreach (var (xPos, yPos) in dict[n])
            {
                if (n == 0)
                {
                    matching.Add((xPos, yPos), [(xPos, yPos)]);
                }

                if (!matching.ContainsKey((xPos, yPos)))
                {
                    continue;
                }

                foreach (var (_, (nXPos, nYPos)) in input
                             .Neighbours(xPos, yPos, true, true, false)
                             .Where(nb => nb.Item1 - '0' == n + 1))
                {
                    foreach (var match in matching[(xPos, yPos)].Where(match => !matching.TryAdd((nXPos, nYPos), [match])))
                    {
                        matching[(nXPos, nYPos)].Add(match);
                    }
                }
            }
        }

        return matching
            .Sum(m => dict[9].Contains(m.Key) ? m.Value.Distinct().Count() : 0)
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();

        var dict = new Dictionary<int, List<(int x, int y)>>();

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var n = input[i][j] - '0';
                if (!dict.TryAdd(n, [(i, j)]))
                {
                    dict[n].Add((i, j));
                }
            }
        }

        var matching = new Dictionary<(int x, int y), List<(int x, int y)>>();
        for (var n = 0; n < 9; n++)
        {
            foreach (var (xPos, yPos) in dict[n])
            {
                if (n == 0)
                {
                    matching.Add((xPos, yPos), [(xPos, yPos)]);
                }

                if (!matching.ContainsKey((xPos, yPos)))
                {
                    continue;
                }

                foreach (var (_, (nXPos, nYPos)) in input
                             .Neighbours(xPos, yPos, true, true, false)
                             .Where(nb => nb.Item1 - '0' == n + 1))
                {
                    foreach (var match in matching[(xPos, yPos)].Where(match => !matching.TryAdd((nXPos, nYPos), [match])))
                    {
                        matching[(nXPos, nYPos)].Add(match);
                    }
                }
            }
        }

        return matching
            .Sum(m => dict[9].Contains(m.Key) ? m.Value.Count : 0)
            .ToString();
    }
}