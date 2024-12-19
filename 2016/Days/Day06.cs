using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var frequencies = new int[input[0].Length][];
        for (var i = 0; i < input[0].Length; i++)
        {
            frequencies[i] = new int[26];
        }

        foreach (var line in input)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                frequencies[i][line[i] - 'a']++;
            }
        }

        return new string(frequencies
            .Select(f => (char)(f.Select((n, i) => (n, i)).MaxBy(x => x.n).i + 'a'))
            .ToArray());
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var frequencies = new int[input[0].Length][];
        for (var i = 0; i < input[0].Length; i++)
        {
            frequencies[i] = new int[26];
        }

        foreach (var line in input)
        {
            for (var i = 0; i < input[0].Length; i++)
            {
                frequencies[i][line[i] - 'a']++;
            }
        }

        return new string(frequencies
            .Select(f => (char)(f.Select((n, i) => (n, i)).MinBy(x => x.n).i + 'a'))
            .ToArray());
    }
}