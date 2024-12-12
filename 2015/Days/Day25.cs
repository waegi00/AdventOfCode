using System.Numerics;
using AdventOfCode.Interfaces;

namespace AdventOfCode._2015.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        const int row = 2981;
        const int column = 3075;
        var grid = new BigInteger[row * 2, column * 2];
        grid[0, 0] = 20151125;

        var n = 2;
        var last = grid[0, 0];
        while (true)
        {
            var i = n - 1;
            var j = 0;

            for (var k = 0; k < n; k++)
            {
                last *= 252533;
                last %= 33554393;
                if (i == row - 1 && j == column - 1)
                {
                    return last.ToString();
                }

                j++;
                i--;
            }

            n++;
        }
    }

    public string SolveSecond()
    {
        return "https://adventofcode.com/2015/day/25#part2";
    }
}