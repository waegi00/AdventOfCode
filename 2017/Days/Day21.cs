using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2017.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(5);
    }

    public string SolveSecond()
    {
        return Solve(18);
    }

    private string Solve(int n)
    {
        var rules = this.InputToLines()
            .Select(x => x.Split(" => ").Select(y => y.Split('/').ToCharArray()).ToArray())
            .Select(x => (from: x[0], to: x[1]))
            .ToList();

        var twos = rules.Where(x => x.from.Length == 2).ToList();
        var threes = rules.Where(x => x.from.Length == 3).ToList();

        var start = ".#.\r\n..#\r\n###".Split("\r\n").ToCharArray();
        var seen = new Dictionary<string, char[][]>();

        for (var r = 0; r < n; r++)
        {
            var size = start[0].Length % 2 == 0 ? 2 : 3;
            var newSize = start.Length + start.Length / size;
            var newStart = new char[newSize][];
            for (var i = 0; i < newSize; i++)
            {
                newStart[i] = new char[newSize];
            }

            for (var i = 0; i < start.Length; i += size)
            {
                for (var j = 0; j < start[i].Length; j += size)
                {
                    var square = new char[size][];
                    for (var s = 0; s < size; s++)
                    {
                        square[s] = start[i + s][j..(j + size)];
                    }

                    var str = new string(square.SelectMany(x => x).ToArray());

                    if (!seen.TryGetValue(str, out var res))
                    {
                        res = FindRule(size == 2 ? twos : threes, square);
                        seen.Add(str, res);
                    }

                    for (var ri = 0; ri < res.Length; ri++)
                    {
                        for (var rj = 0; rj < res[ri].Length; rj++)
                        {
                            newStart[i + i / size + ri][j + j / size + rj] = res[ri][rj];
                        }
                    }
                }
            }

            start = newStart;
        }

        return start.Sum(x => x.Count(y => y == '#')).ToString();
    }

    private static char[][] FindRule(List<(char[][] from, char[][] to)> rules, char[][] square)
    {
        foreach (var rule in rules)
        {
            for (var i = 0; i < 4; i++)
            {
                if (rule.from.EqualValues(square) || rule.from.FlipRows().EqualValues(square))
                {
                    return rule.to;
                }

                square = square.Rotate90DegreesRight();
            }
        }

        return square;
    }
}