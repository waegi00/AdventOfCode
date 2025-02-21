using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        return Apply(input).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        return Apply(input, true).ToString();
    }

    private static int Apply(int[] mem, bool isPart2 = false)
    {
        var address = 758;
        var sum = 0;

        Next();

        if (isPart2) Next();

        return sum;

        void Next()
        {
            while (true)
            {
                sum += mem[address] * address * Holes(mem[address]);
                address++;
                if (mem[address] != 0)
                {
                    continue;
                }

                break;
            }
        }
    }

    private static int Holes(int v)
    {
        var sum = 0;
        for (var x = 0; x <= 8; x++)
        {
            if ((v & (1 << x)) == 0)
            {
                sum += 18 - x;
            }
        }
        return sum;
    }
}