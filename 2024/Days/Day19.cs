using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var availableTowels = input[0].Split(", ").ToHashSet();
        var designs = input[1].Split("\r\n");

        return designs.Count(design => IsPossibleDesign(design, availableTowels)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var availableTowels = input[0].Split(", ").ToHashSet();
        var designs = input[1].Split("\r\n");

        return designs.Sum(design => GetPossibleDesigns(design, availableTowels, [])).ToString();
    }

    private static bool IsPossibleDesign(string design, HashSet<string> availableTowels)
    {
        if (availableTowels.Contains(design))
        {
            return true;
        }

        for (var i = 1; i < design.Length; i++)
        {
            if (!availableTowels.Contains(design[i..]) || !IsPossibleDesign(design[..i], availableTowels)) continue;

            return true;
        }

        return false;
    }

    private static long GetPossibleDesigns(string design, HashSet<string> availableTowels, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(design, out var value))
        {
            return value;
        }

        var res = 0L;

        if (availableTowels.Contains(design))
        {
            res++;
        }

        for (var i = 1; i < design.Length; i++)
        {
            if (!availableTowels.Contains(design[i..])) continue;

            if (!cache.TryGetValue(design[..i], out var amount))
            {
                amount = GetPossibleDesigns(design[..i], availableTowels, cache);
                cache.Add(design[..i], amount);
            }
            res += amount;
        }

        return res;
    }
}