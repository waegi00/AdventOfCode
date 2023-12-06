using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day3 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day3.txt");

        int x = input.Length, y = input[0].Length;
        var data = new char[x, y];

        for (var i = 0; i < x; i++)
        {
            var line = input[i];

            for (var j = 0; j < y; j++)
            {
                data[i, j] = line[j];
            }
        }

        var sum = 0;

        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                if (!char.IsNumber(data[i, j])) continue;
                var hasNeighbour = HasNeighbour(data, i, j);
                var num = int.Parse(data[i, j].ToString());

                while (++j < y && char.IsNumber(data[i, j]))
                {
                    num *= 10;
                    num += int.Parse(data[i, j].ToString());
                    if (HasNeighbour(data, i, j))
                    {
                        hasNeighbour = true;
                    }
                }

                if (hasNeighbour)
                {
                    sum += num;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day3.txt");

        int x = input.Length, y = input[0].Length;
        var data = new char[x, y];

        for (var i = 0; i < x; i++)
        {
            var line = input[i];

            for (var j = 0; j < y; j++)
            {
                data[i, j] = line[j];
            }
        }

        var sum = 0;

        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                if (data[i, j] != '*') continue;

                sum += GetNumsAround(data, i, j);
            }
        }

        return sum.ToString();
    }

    private static bool HasNeighbour(char[,] data, int x, int y)
    {
        if (x >= data.GetLength(0) || y >= data.GetLength(1))
        {
            return false;
        }

        for (var i = -1; i <= 1 && x + i < data.GetLength(0); i++)
        {
            if (x + i < 0) continue;
            for (var j = -1; j <= 1 && y + j < data.GetLength(1); j++)
            {
                if (y + j < 0) continue;
                var val = data[x + i, y + j];
                if (!char.IsNumber(val) && val != '.')
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static int GetNumsAround(char[,] data, int x, int y)
    {
        if (x >= data.GetLength(0) || y >= data.GetLength(1))
        {
            return 0;
        }

        var a = 0;
        var b = 0;

        for (var i = -1; i <= 1 && x + i < data.GetLength(0); i++)
        {
            if (x + i < 0) continue;
            for (var j = -1; j <= 1 && y + j < data.GetLength(1); j++)
            {
                if (y + j < 0) continue;
                var val = data[x + i, y + j];
                if (!char.IsNumber(val)) continue;
                if (a == 0)
                {
                    a = FindNumber(data, x + i, y + j);
                    while (j <= 1 && char.IsNumber(data[x + i, y + j]))
                    {
                        j++;
                    }
                }
                else
                {
                    return a * FindNumber(data, x + i, y + j);
                }
            }
        }

        return 0;
    }

    private static int FindNumber(char[,] data, int x, int y)
    {
        if (x >= data.GetLength(0) || y >= data.GetLength(1))
        {
            return 0;
        }

        while (y >= 1 && char.IsNumber(data[x, y - 1]))
        {
            y--;
        }

        var result = 0;
        while (y < data.GetLength(1) && char.IsNumber(data[x, y]))
        {
            result *= 10;
            result += int.Parse(data[x, y].ToString());
            y++;
        }

        return result;
    }
}
