using System.Numerics;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n").Select(x => x.Split("\r\n"));

        var cost = 0;
        foreach (var game in input)
        {
            var data = game.Select(g => g.Split(": ")[1].Split(", ")).ToList();

            var (ax, ay) = (int.Parse(data[0][0][1..]), int.Parse(data[0][1][1..]));
            var (bx, by) = (int.Parse(data[1][0][1..]), int.Parse(data[1][1][1..]));
            var (x, y) = (int.Parse(data[2][0][2..]), int.Parse(data[2][1][2..]));

            var best = int.MaxValue;
            for (var i = 1; i <= 100 && i * ax <= x && i * ay <= y; i++)
            {
                var (dx, dy) = (x - i * ax, y - i * ay);
                if (dx % bx == 0 && dy % by == 0 && dy / by == dx / bx)
                {
                    best = Math.Min(best, 3 * i + dy / by);
                }
            }

            if (best != int.MaxValue)
            {
                cost += best;
            }
        }

        return cost.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText().Split("\r\n\r\n").Select(x => x.Split("\r\n"));

        var cost = 0L;
        foreach (var game in input)
        {
            var data = game.Select(g => g.Split(": ")[1].Split(", ")).ToList();

            var (ax, ay) = (long.Parse(data[0][0][1..]), long.Parse(data[0][1][1..]));
            var (bx, by) = (long.Parse(data[1][0][1..]), long.Parse(data[1][1][1..]));
            var (x, y) = (long.Parse(data[2][0][2..]) + 10000000000000, long.Parse(data[2][1][2..]) + 10000000000000);

            var numA = x * by - y * bx;
            var denA = ax * by - ay * bx;

            var numB = x * ay - y * ax;
            var denB = ay * bx - ax * by;

            if (numA % denA != 0 || numB % denB != 0) continue;
            var a = numA / denA;
            var b = numB / denB;
            cost += 3 * a + b;
        }

        return cost.ToString();
    }
}