using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public partial class Day07 : IRiddle
{
    private readonly Dictionary<string, int> _cache = new();

    public string SolveFirst()
    {
        var input = this.InputToLines();
        _cache.Clear();

        var instructions = new Dictionary<string, string>();

        foreach (var line in input)
        {
            var parts = line.Split([" -> "], StringSplitOptions.None);
            instructions[parts[1]] = parts[0];
        }

        return Evaluate("a", instructions).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();
        _cache.Clear();

        var instructions = new Dictionary<string, string>();

        foreach (var line in input)
        {
            var parts = line.Split([" -> "], StringSplitOptions.None);
            instructions[parts[1]] = parts[0];
        }

        var a = Evaluate("a", instructions);
        _cache.Clear();
        _cache.Add("b", a);

        return Evaluate("a", instructions).ToString();
    }

    private int Evaluate(string wire, Dictionary<string, string> instructions)
    {
        if (_cache.TryGetValue(wire, out var cacheValue))
        {
            return cacheValue;
        }

        if (int.TryParse(wire, out var wireValue))
        {
            _cache[wire] = wireValue;
            return wireValue;
        }

        var instruction = instructions[wire];

        if (int.TryParse(instruction, out var instructionValue))
        {
            _cache[wire] = instructionValue;
            return instructionValue;
        }

        var value = 0;
        if (MyRegex().IsMatch(instruction))
        {
            value = Evaluate(instruction, instructions);
        }
        else if (instruction.StartsWith("NOT"))
        {
            var subWire = instruction.Split(' ')[1];
            value = ~Evaluate(subWire, instructions) & 0xFFFF;
        }
        else if (instruction.Contains("AND"))
        {
            var splits = instruction.Split([" AND "], StringSplitOptions.None);
            value = Evaluate(splits[0], instructions) & Evaluate(splits[1], instructions);
        }
        else if (instruction.Contains("OR"))
        {
            var splits = instruction.Split([" OR "], StringSplitOptions.None);
            value = Evaluate(splits[0], instructions) | Evaluate(splits[1], instructions);
        }
        else if (instruction.Contains("LSHIFT"))
        {
            var splits = instruction.Split([" LSHIFT "], StringSplitOptions.None);
            value = Evaluate(splits[0], instructions) << int.Parse(splits[1]);
        }
        else if (instruction.Contains("RSHIFT"))
        {
            var splits = instruction.Split([" RSHIFT "], StringSplitOptions.None);
            value = Evaluate(splits[0], instructions) >> int.Parse(splits[1]);
        }

        _cache[wire] = value;
        return value;
    }

    [GeneratedRegex(@"^[a-z]+$")]
    private static partial Regex MyRegex();
}