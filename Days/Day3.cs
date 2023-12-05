using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day3 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day3.txt");

        int x = input.Length, y = input[0].Length;
        char[,] data = new char[x, y];

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
                if (char.IsNumber(data[i, j]))
                {
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
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day3.txt");

        return "";
    }

    private static bool HasNeighbour(char[,] data, int x, int y)
    {
        if (x >= data.GetLength(0) || y >= data.GetLength(1))
        {
            return false;
        }

        for (var i = -1; i <= 1 && x + i < data.GetLength(0); i++)
        {
            if (x + i >= 0)
            {
                for (var j = -1; j <= 1 && y + j < data.GetLength(1); j++)
                {
                    if (y + j >= 0)
                    {
                        var val = data[x + i, y + j];
                        if (!char.IsNumber(val) && val != '.')
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
