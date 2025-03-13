using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day21 : IRiddle
{

    public string SolveFirst()
    {
        var players = this.InputToLines()
            .Select(x => (pos: x[^1] - '0', score: 0))
            .ToArray();
        var die = 0;
        var rolls = 0;

        while (players.All(p => p.score < 1000))
        {
            die = die % 100 + 1;
            players[(die - 1) % 2].pos += 3 * die + 3;
            players[(die - 1) % 2].score += (players[(die - 1) % 2].pos - 1) % 10 + 1;
            die += 2;
            rolls += 3;
        }

        return (players.Min(p => p.score) * rolls).ToString();
    }

    public string SolveSecond()
    {
        var players = this.InputToLines()
            .Select(x => (pos: x[^1] - '0', score: 0))
            .ToArray();

        var result = PlayOut(players[0], players[1], []);
        return Math.Max(result.Item1, result.Item2).ToString();
    }

    private static (long, long) PlayOut((int p, int s) p1, (int p, int s) p2, Dictionary<((int, int), (int, int)), (long, long)> memo)
    {
        if (memo.TryGetValue((p1, p2), out var cachedResult))
        {
            return cachedResult;
        }

        long w1 = 0, w2 = 0;

        for (var m1 = 1; m1 <= 3; m1++)
        {
            for (var m2 = 1; m2 <= 3; m2++)
            {
                for (var m3 = 1; m3 <= 3; m3++)
                {
                    var p1Copy = (p1.p + m1 + m2 + m3) % 10;
                    if (p1Copy == 0)
                    {
                        p1Copy = 10;
                    }
                    var s1Copy = p1.s + p1Copy;

                    if (s1Copy >= 21)
                    {
                        w1++;
                    }
                    else
                    {
                        var (w2Copy, w1Copy) = PlayOut(p2, (p1Copy, s1Copy), memo);
                        w1 += w1Copy;
                        w2 += w2Copy;
                    }
                }
            }
        }

        var result = (w1, w2);
        memo[(p1, p2)] = result;
        return result;
    }
}