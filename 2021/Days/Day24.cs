using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(Enumerable.Repeat(9, 14).ToArray(), this.InputToLines());
    }

    public string SolveSecond()
    {
        return Solve(Enumerable.Repeat(1, 14).ToArray(), this.InputToLines());
    }

    private static string Solve(int[] inp, string[] lines)
    {
        var commands = lines
            .Select(line => line.Split(' '))
            .ToList();

        var stack = new Stack<(int, int)>();
        for (var i = 0; i < 14; i++)
        {
            var div = int.Parse(commands[i * 18 + 4][^1]);
            var chk = int.Parse(commands[i * 18 + 5][^1]);
            var add = int.Parse(commands[i * 18 + 15][^1]);

            switch (div)
            {
                case 1:
                    stack.Push((i, add));
                    break;
                case 26:
                    {
                        var (j, addValue) = stack.Pop();
                        inp[i] = inp[j] + addValue + chk;
                        if (inp[i] > 9)
                        {
                            inp[j] -= inp[i] - 9;
                            inp[i] = 9;
                        }
                        if (inp[i] < 1)
                        {
                            inp[j] += 1 - inp[i];
                            inp[i] = 1;
                        }

                        break;
                    }
            }
        }

        return string.Join("", inp);
    }
}