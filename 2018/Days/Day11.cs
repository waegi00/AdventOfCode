using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var serialNumber = int.Parse(this.InputToText());

        const int size = 300;
        var grid = new long[size][];
        for (var i = 0; i < size; i++)
        {
            grid[i] = new long[size];
            for (var j = 0; j < size; j++)
            {
                grid[i][j] = PowerLevel(i, j, serialNumber);
            }
        }

        var max = 0L;
        var res = string.Empty;

        for (var i = 0; i < size - 2; i++)
        {
            for (var j = 0; j < size - 2; j++)
            {
                var curr = 0L;

                for (var x = 0; x < 3; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        curr += grid[i + x][j + y];
                    }
                }

                if (curr <= max) continue;
                max = curr;
                res = $"{i + 1},{j + 1}";
            }
        }

        return res;
    }

    public string SolveSecond()
    {
        var serialNumber = int.Parse(this.InputToText());

        const int size = 300;
        var grid = new long[size, size];
        var partialSums = new long[size + 1, size + 1];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                grid[i, j] = PowerLevel(i, j, serialNumber);
            }
        }

        for (var i = 1; i <= size; i++)
        {
            for (var j = 1; j <= size; j++)
            {
                partialSums[i, j] = grid[i - 1, j - 1] +
                                    partialSums[i - 1, j] +
                                    partialSums[i, j - 1] -
                                    partialSums[i - 1, j - 1];
            }
        }

        var max = long.MinValue;
        var res = string.Empty;

        for (var s = 1; s <= size; s++)
        {
            for (var i = 0; i <= size - s; i++)
            {
                for (var j = 0; j <= size - s; j++)
                {
                    var curr = partialSums[i + s, j + s] - 
                               partialSums[i, j + s] -
                               partialSums[i + s, j] +
                               partialSums[i, j];

                    if (curr <= max) continue;
                    max = curr;
                    res = $"{i + 1},{j + 1},{s}";
                }
            }
        }

        return res;
    }

    private static long PowerLevel(int x, int y, int serialNumber)
    {
        var rackId = x + 1L + 10;
        var powerLevel = rackId * (y + 1);
        powerLevel += serialNumber;
        powerLevel *= rackId;
        powerLevel %= 1000;
        powerLevel /= 100;
        return powerLevel - 5;
    }
}