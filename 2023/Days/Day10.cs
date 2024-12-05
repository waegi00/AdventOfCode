using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day10.txt");

        for (var x = 0; x < input.Length; x++)
        {
            for (var y = 0; y < input[x].Length; y++)
            {
                if (input[x][y] != 'S') continue;

                var c = 0;
                var val = input[x][y];
                var i = x;
                var j = y;
                var dir = Direction.None;

                do
                {
                    if (val == 'S')
                    {
                        if (i > 0 && new List<char> { '|', 'F', '7' }.Contains(input[i - 1][j]))
                        {
                            val = input[--i][j];
                            dir = Direction.Bottom;
                        }
                        else if (i < input.Length - 1 && new List<char> { '|', 'L', 'J' }.Contains(input[i + 1][j]))
                        {
                            val = input[++i][j];
                            dir = Direction.Top;
                        }
                        else if (j < input[i].Length - 1 && new List<char> { '-', 'J', '7' }.Contains(input[i][j + 1]))
                        {
                            val = input[i][++j];
                            dir = Direction.Left;
                        }
                        else if (j > 0 && new List<char> { '-', 'F', 'L' }.Contains(input[i][j - 1]))
                        {
                            val = input[i][--j];
                            dir = Direction.Right;
                        }
                    }
                    else
                    {
                        switch (val)
                        {
                            case '|':
                                val = dir == Direction.Bottom ? input[--i][j] : input[++i][j];
                                break;
                            case '-':
                                val = dir == Direction.Left ? input[i][++j] : input[i][--j];
                                break;
                            case 'L':
                                val = dir == Direction.Top ? input[i][++j] : input[--i][j];
                                dir = dir == Direction.Top ? Direction.Left : Direction.Bottom;
                                break;
                            case 'J':
                                val = dir == Direction.Top ? input[i][--j] : input[--i][j];
                                dir = dir == Direction.Top ? Direction.Right : Direction.Bottom;
                                break;
                            case '7':
                                val = dir == Direction.Bottom ? input[i][--j] : input[++i][j];
                                dir = dir == Direction.Bottom ? Direction.Right : Direction.Top;
                                break;
                            case 'F':
                                val = dir == Direction.Bottom ? input[i][++j] : input[++i][j];
                                dir = dir == Direction.Bottom ? Direction.Left : Direction.Top;
                                break;
                        }
                    }

                    c++;
                } while (val != 'S');

                return (c / 2).ToString();
            }
        }

        throw new Exception("Input contains no loop");
    }

    public string SolveSecond()
    {
        return "";
    }

    private enum Direction
    {
        None,
        Top,
        Left,
        Right,
        Bottom
    }
}
