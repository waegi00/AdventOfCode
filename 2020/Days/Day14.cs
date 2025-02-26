using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public partial class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var memory = new Dictionary<long, long>();
        var mask = string.Empty;

        foreach (var line in this.InputToLines())
        {
            var parts = line.Split(" = ");
            if (parts[0] == "mask")
            {
                mask = parts[1];
            }
            else
            {
                var address = long.Parse(DigitRegex().Match(parts[0]).Value);
                var value = long.Parse(parts[1]);

                memory[address] = (value & Convert.ToInt64(mask.Replace('1', '0').Replace('X', '1'), 2)) | Convert.ToInt64(mask.Replace('X', '0'), 2);
            }
        }

        return memory.Values.Sum().ToString();
    }

    public string SolveSecond()
    {
        var memory = new Dictionary<long, long>();
        var mask = string.Empty;

        foreach (var line in this.InputToLines())
        {
            var parts = line.Split(" = ");
            if (parts[0] == "mask")
            {
                mask = parts[1];
            }
            else
            {
                var address = long.Parse(DigitRegex().Match(parts[0]).Value) & Convert.ToInt64(mask.Replace('0', '1').Replace('X', '0'), 2);
                var value = long.Parse(parts[1]);
                Write(memory, mask, address, value);
            }
        }
        return memory.Values.Sum().ToString();
    }

    private static void Write(Dictionary<long, long> memory, string mask, long address, long value)
    {
        while (true)
        {
            if (mask.Contains('X'))
            {
                var i = mask.IndexOf('X');
                Write(memory, string.Concat(mask.AsSpan(0, i), "0", mask.AsSpan(i + 1)), address, value);
                mask = string.Concat(mask.AsSpan(0, i), "1", mask.AsSpan(i + 1));
                continue;
            }

            memory[Convert.ToInt64(mask, 2) | address] = value;

            break;
        }
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex DigitRegex();
}