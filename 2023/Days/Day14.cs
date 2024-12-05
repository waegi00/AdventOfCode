using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day14.txt");

        var dish = input.Select(x => x.ToArray()).ToArray();

        var sum = 0;

        for (var i = 0; i < dish.Length; i++)
        {
            for (var j = 0; j < dish[i].Length; j++)
            {
                if (dish[i][j] == 'O')
                {
                    var n = 1;
                    while (i - n >= 0 && dish[i - n][j] == '.')
                    {
                        n++;
                    }
                    if (n != 1)
                    {
                        (dish[i - n + 1][j], dish[i][j]) = (dish[i][j], dish[i - n + 1][j]);
                    }

                    sum += dish[i].Length - i + n - 1;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day14.txt");

        var dish = input.Select(x => x.ToArray()).ToArray();
        var hashes = new Dictionary<int, int>();

        for (var k = 1; k <= 1_000_000_000; k++)
        {
            for (var i = 0; i < dish.Length; i++)
            {
                for (var j = 0; j < dish[i].Length; j++)
                {
                    if (dish[i][j] == 'O')
                    {
                        var n = 1;
                        while (i - n >= 0 && dish[i - n][j] == '.')
                        {
                            n++;
                        }
                        if (n != 1)
                        {
                            (dish[i - n + 1][j], dish[i][j]) = (dish[i][j], dish[i - n + 1][j]);
                        }
                    }
                }
            }

            for (var i = 0; i < dish.Length; i++)
            {
                for (var j = 0; j < dish[i].Length; j++)
                {
                    if (dish[i][j] == 'O')
                    {
                        var n = 1;
                        while (j - n >= 0 && dish[i][j - n] == '.')
                        {
                            n++;
                        }
                        if (n != 1)
                        {
                            (dish[i][j - n + 1], dish[i][j]) = (dish[i][j], dish[i][j - n + 1]);
                        }
                    }
                }
            }

            for (var i = dish.Length - 1; i >= 0; i--)
            {
                for (var j = 0; j < dish[i].Length; j++)
                {
                    if (dish[i][j] == 'O')
                    {
                        var n = 1;
                        while (i + n < dish.Length && dish[i + n][j] == '.')
                        {
                            n++;
                        }
                        if (n != 1)
                        {
                            (dish[i + n - 1][j], dish[i][j]) = (dish[i][j], dish[i + n - 1][j]);
                        }
                    }
                }
            }

            for (var i = 0; i < dish.Length; i++)
            {
                for (var j = dish[i].Length - 1; j >= 0; j--)
                {
                    if (dish[i][j] == 'O')
                    {
                        var n = 1;
                        while (j + n < dish[0].Length && dish[i][j + n] == '.')
                        {
                            n++;
                        }
                        if (n != 1)
                        {
                            (dish[i][j + n - 1], dish[i][j]) = (dish[i][j], dish[i][j + n - 1]);
                        }
                    }
                }
            }

            if (hashes.TryGetValue(dish.HashValue(), out var seenAt))
            {
                var diff = k - seenAt;
                while (k + diff < 1_000_000_000)
                {
                    k += diff;
                }
            }
            else
            {
                hashes.Add(dish.HashValue(), k);
            }
        }

        var sum = 0;

        for (var i = 0; i < dish.Length; i++)
        {
            sum += (dish.Length - i) * dish[i].Count(x => x == 'O');
        }

        return sum.ToString();
    }
}

public static class CharMatrixExtensions
{
    public static int HashValue(this char[][] matrix)
    {
        int hash = 17;
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                hash = hash * 31 + matrix[i][j];
            }
        }
        return hash;
    }
}