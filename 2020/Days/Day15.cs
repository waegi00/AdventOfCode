using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        return NthNumberSpoken(2020).ToString();
    }

    public string SolveSecond()
    {
        return NthNumberSpoken(30000000).ToString();
    }

    private int NthNumberSpoken(int n)
    {
        var spoken = this.InputToText()
            .Split(',')
            .Select((x, i) => (val: int.Parse(x), index: i))
            .ToDictionary(x => x.val, x => (x.index, count: 1));

        var lastSpoken = 0;
        for (var i = spoken.Count; i < n - 1; i++)
        {
            if (spoken.TryGetValue(lastSpoken, out var curr))
            {
                spoken[lastSpoken] = (i, curr.count + 1);
                lastSpoken = i - curr.index;
            }
            else
            {
                spoken[lastSpoken] = (i, 1);
                lastSpoken = 0;
            }
        }

        return lastSpoken;
    }
}