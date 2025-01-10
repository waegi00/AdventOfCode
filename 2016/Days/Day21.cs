using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        return Solve("abcdefgh".ToList());
    }

    public string SolveSecond()
    {
        const string expected = "fbgdceah";

        foreach (var p in expected
                     .GetPermutations(expected.Length)
                     .Select(x => x.ToList())
                     .Where(x => Solve(x.ToList()) == expected))
        {
            return new string(p.ToArray());
        }

        return "";
    }

    private string Solve(List<char> result)
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToList();

        foreach (var splits in input)
        {
            _ = int.TryParse(splits[2], out var x);
            _ = int.TryParse(splits[^1], out var y);

            switch (splits[0])
            {
                case "move":
                    var move = result[x];
                    result.RemoveAt(x);
                    result.Insert(y, move);
                    break;
                case "reverse":
                    var reverse = result[x..(y + 1)];
                    reverse.Reverse();
                    result.RemoveRange(x, y - x + 1);
                    result.InsertRange(x, reverse);
                    break;
                case "rotate":
                    switch (splits[1])
                    {
                        case "left":
                            var left = result[..x];
                            result.RemoveRange(0, x);
                            result.AddRange(left);
                            break;
                        case "based":
                            x = 1 + result.IndexOf(splits[^1][0]);
                            if (x >= 5)
                            {
                                x++;
                            }
                            x %= result.Count;

                            var based = result[^x..];
                            result.RemoveRange(result.Count - x, x);
                            result.InsertRange(0, based);
                            break;
                        case "right":
                            var right = result[^x..];
                            result.RemoveRange(result.Count - x, x);
                            result.InsertRange(0, right);
                            break;
                    }
                    break;
                case "swap":
                    switch (splits[1])
                    {
                        case "position":
                            (result[x], result[y]) = (result[y], result[x]);
                            break;
                        case "letter":
                            result = result
                                .Select(c => c == splits[2][0] ? '\0' : c)
                                .Select(c => c == splits[^1][0] ? splits[2][0] : c)
                                .Select(c => c == '\0' ? splits[^1][0] : c)
                                .ToList();
                            break;
                    }
                    break;
            }
        }

        return new string(result.ToArray());
    }
}