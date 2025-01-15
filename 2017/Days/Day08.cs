using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' ').ToList());

        var registers = new Dictionary<string, int>();

        foreach (var line in input)
        {
            var reg = line[0];
            var isInc = line[1] == "inc";
            var amount = int.Parse(line[2]);
            var a = registers.GetValueOrDefault(line[4], 0);
            var b = int.Parse(line[^1]);

            if (!(line[5] switch
            {
                "<" => a < b,
                ">" => a > b,
                "==" => a == b,
                ">=" => a >= b,
                "<=" => a <= b,
                "!=" => a != b,
                _ => throw new ArgumentOutOfRangeException()
            })) continue;

            var n = isInc ? amount : -amount;
            if (!registers.TryAdd(reg, n))
            {
                registers[reg] += n;
            }
        }

        return registers.Values.Max().ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' ').ToList());

        var registers = new Dictionary<string, int>();
        var max = 0;

        foreach (var line in input)
        {
            var reg = line[0];
            var isInc = line[1] == "inc";
            var amount = int.Parse(line[2]);
            var a = registers.GetValueOrDefault(line[4], 0);
            var b = int.Parse(line[^1]);

            if (!(line[5] switch
            {
                "<" => a < b,
                ">" => a > b,
                "==" => a == b,
                ">=" => a >= b,
                "<=" => a <= b,
                "!=" => a != b,
                _ => throw new ArgumentOutOfRangeException()
            })) continue;

            var n = isInc ? amount : -amount;
            if (!registers.TryAdd(reg, n))
            {
                registers[reg] += n;
            }

            if (registers[reg] > max)
            {
                max = registers[reg];
            }
        }

        return max.ToString();
    }
}