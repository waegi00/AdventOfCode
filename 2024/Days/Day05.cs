using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n");

        var ordering = input[0].Split("\r\n");
        var updates = input[1].Split("\r\n").Select(u => u.Split(',').Select(int.Parse).ToList());

        var dict = new Dictionary<int, List<int>>();
        foreach (var (k, v) in ordering.Select(o => o.Split('|')).Select(s => (int.Parse(s[0]), int.Parse(s[1]))))
        {
            if (!dict.TryAdd(k, [v]))
            {
                dict[k].Add(v);
            }
        }

        var sum = 0;

        foreach (var update in updates)
        {
            for (var i = 0; i < update.Count - 1; i++)
            {
                if (dict.TryGetValue(update[i + 1], out var l) && l.Contains(update[i]))
                {
                    i = update.Count;
                }
                else if (i == update.Count - 2)
                {
                    sum += update[update.Count / 2];
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText().Split("\r\n\r\n");

        var ordering = input[0].Split("\r\n");
        var updates = input[1].Split("\r\n").Select(u => u.Split(',').Select(int.Parse).ToList());

        var dict = new Dictionary<int, List<int>>();
        foreach (var (k, v) in ordering.Select(o => o.Split('|')).Select(s => (int.Parse(s[0]), int.Parse(s[1]))))
        {
            if (!dict.TryAdd(k, [v]))
            {
                dict[k].Add(v);
            }
        }

        var incorrectUpdates = new List<List<int>>();

        foreach (var update in updates)
        {
            for (var i = 0; i < update.Count - 1; i++)
            {
                if (!dict.TryGetValue(update[i + 1], out var l) || !l.Contains(update[i])) continue;

                update.Sort((a, b) => dict.TryGetValue(a, out var l) && l.Contains(b) ? 1 : -1);
                incorrectUpdates.Add(update);
                i = update.Count;
            }
        }

        return incorrectUpdates.Sum(u => u[u.Count / 2]).ToString();
    }
}