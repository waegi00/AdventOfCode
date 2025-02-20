using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .ToCharArray()
            .Select(x => x - '0')
            .ToArray();

        for (var i = 0; i < 100; i++)
        {
            input = Enumerable.Range(0, input.Length)
                .Select(x => Compute(input, x))
                .ToArray();
        }

        return string.Join("", input.Take(8));
    }
    public string SolveSecond()
    {
        var input = this.InputToText()
            .ToCharArray()
            .Select(x => x - '0')
            .ToArray();

        var offset = int.Parse(string.Join("", input.Take(7)));

        input = Enumerable.Repeat(input, 10000)
            .SelectMany(x => x)
            .Skip(offset)
            .ToArray();

        for (var i = 0; i < 100; i++)
        {
            var sum = input.Sum();
            var output = new int[input.Length];
            for (var n = 0; n < input.Length; n++)
            {
                output[n] = (sum % 10 + 10) % 10;
                sum -= input[n];
            }
            input = output;
        }

        return string.Join("", input.Take(8));
    }

    private static int Compute(int[] numbers, int index)
    {
        index += 1;
        var result = numbers
            .Select((x, i) => x * GetMultiplier(index, i))
            .Sum();
        return Math.Abs(result) % 10;
    }

    private static int GetMultiplier(int n, int m)
    {
        m += 1;
        m %= 4 * n;
        if (m < n) return 0;
        if (m < 2 * n) return 1;
        if (m < 3 * n) return 0;
        return -1;
    }
}