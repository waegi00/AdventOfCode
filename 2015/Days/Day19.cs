using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var medicine = input[1];
        var replacements = input[0].Split("\r\n")
            .Select(line => line.Split(" => "))
            .GroupBy(parts => parts[0], parts => parts[1])
            .ToDictionary(group => group.Key, group => group.ToList());

        var molecules = new HashSet<string>();
        foreach (var r in replacements)
        {
            var regex = new Regex($"{r.Key}");
            foreach (var value in r.Value)
            {
                foreach (Match match in regex.Matches(medicine))
                {
                    molecules.Add(medicine[..match.Index] + value + medicine[(match.Index + r.Key.Length)..]);
                }
            }
        }
        return molecules.Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var medicine = input[1];
        var replacements = input[0].Split("\r\n")
            .Select(line => line.Split(" => "))
            .ToDictionary(parts => parts[1], parts => parts[0]);

        var steps = 0;
        while (medicine != "e")
        {
            foreach (var (from, to) in replacements)
            {
                var index = medicine.IndexOf(from, StringComparison.Ordinal);
                if (index == -1) continue;
                medicine = medicine[..index] + to + medicine[(index + from.Length)..];
                steps++;
                break;
            }
        }

        return steps.ToString();
    }
}