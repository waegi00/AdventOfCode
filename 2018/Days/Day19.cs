using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public partial class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToArray();

        var registers = new int[6];
        var ip = int.Parse(input[0][1]);

        input = input[1..];
        for (var i = 0; i >= 0 && i < input.Length; i++)
        {
            registers[ip] = i;
            Apply(
                registers,
                input[i][0],
                int.Parse(input[i][1]),
                int.Parse(input[i][2]),
                int.Parse(input[i][3]));
            i = registers[ip];
        }

        return registers[0].ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var a = int.Parse(NumberRegex().Matches(input[22])[1].Value);
        var b = int.Parse(NumberRegex().Matches(input[24])[1].Value);

        long numberToFactorize = 10551236 + a * 22 + b;

        var factors = new Dictionary<long, int>();
        long possiblePrimeDivisor = 2;

        while (possiblePrimeDivisor * possiblePrimeDivisor <= numberToFactorize)
        {
            while (numberToFactorize % possiblePrimeDivisor == 0)
            {
                numberToFactorize /= possiblePrimeDivisor;
                if (!factors.TryAdd(possiblePrimeDivisor, 1))
                {
                    factors[possiblePrimeDivisor]++;
                }
            }
            possiblePrimeDivisor++;
        }

        if (numberToFactorize > 1)
        {
            if (!factors.TryAdd(numberToFactorize, 1))
            {
                factors[numberToFactorize]++;
            }
        }

        var sumOfDivisors = factors.Aggregate<KeyValuePair<long, int>, double>(
            1,
            (current, primeFactor) =>
                current * ((Math.Pow(primeFactor.Key, primeFactor.Value + 1) - 1) / (primeFactor.Key - 1)));

        return ((int)sumOfDivisors).ToString();
    }

    private static void Apply(int[] registers, string op, int a, int b, int c)
    {
        registers[c] = op switch
        {
            "addr" => registers[a] + registers[b],
            "addi" => registers[a] + b,
            "mulr" => registers[a] * registers[b],
            "muli" => registers[a] * b,
            "banr" => registers[a] & registers[b],
            "bani" => registers[a] & b,
            "borr" => registers[a] | registers[b],
            "bori" => registers[a] | b,
            "setr" => registers[a],
            "seti" => a,
            "gtir" => a > registers[b] ? 1 : 0,
            "gtri" => registers[a] > b ? 1 : 0,
            "gtrr" => registers[a] > registers[b] ? 1 : 0,
            "eqir" => a == registers[b] ? 1 : 0,
            "eqri" => registers[a] == b ? 1 : 0,
            "eqrr" => registers[a] == registers[b] ? 1 : 0,
            _ => registers[c]
        };
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();
}