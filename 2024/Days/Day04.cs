using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2024.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().ToCharArray();

        var sum = 0;

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != 'X') continue;

                var ns = input.Neighbours(i, j).ToList();
                foreach (var (_, (im, jm)) in ns.Where(n => n.Item1 == 'M'))
                {
                    var (di, dj) = (im - i, jm - j);
                    if (input.IsValidPosition(im + di, jm + dj) && input[im + di][jm + dj] == 'A' &&
                        input.IsValidPosition(im + 2 * di, jm + 2 * dj) && input[im + 2 * di][jm + 2 * dj] == 'S')
                    {
                        sum++;
                    }
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();

        var sum = 0;

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != 'M') continue;

                var ns = input.Neighbours(i, j, false, false).ToList();
                foreach (var (_, (ia, ja)) in ns.Where(n => n.Item1 == 'A'))
                {
                    var (di, dj) = (ia - i, ja - j);
                    if (!input.IsValidPosition(ia + di, ja + dj) || input[ia + di][ja + dj] != 'S') continue;
                    if (!input.IsValidPosition(ia + -1 * di, ja + dj) || !input.IsValidPosition(ia + di, ja + -1 * dj)) continue;

                    var a = input[ia + -1 * di][ja + dj];
                    var b = input[ia + di][ja + -1 * dj];

                    if (a != b && a is 'S' or 'M' && b is 'S' or 'M')
                    {
                        sum++;
                    }
                }
            }
        }

        return (sum / 2).ToString();
    }
}
