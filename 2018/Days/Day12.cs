using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        return PotSum(20).ToString();
    }

    public string SolveSecond()
    {
        return PotSum(50000000000).ToString();
    }

    private long PotSum(long gen)
    {
        var input = this.InputToText()
            .Split("\r\n\r\n");

        var pots = input[0].Split(": ")[1]
            .Select((x, i) => (x == '#', i))
            .Where(x => x.Item1)
            .Select(x => x.i)
            .ToHashSet();

        var spreads = input[1].Split("\r\n")
            .Select(x => x.Split(" => "))
            .Where(x => x[1][0] == '#')
            .Select(x => (Pattern: x[0].Select(c => c == '#').ToArray(), To: x[1][0] == '#'))
            .ToArray();

        var history = new List<int>();
        for (var i = 1; i <= gen; i++)
        {
            var newPots = new HashSet<int>();

            for (var p = pots.Min() - 2; p <= pots.Max() + 1; p++)
            {
                var a = Enumerable.Range(p - 2, 5).Select(pots.Contains).ToArray();

                if (spreads.Any(s => s.Pattern.SequenceEqual(a)))
                {
                    newPots.Add(p);
                }
            }

            var prev = pots.Sum();
            pots = newPots;
            history.Add(pots.Sum() - prev);
            if (history.Count > 10 && history[^10..].Distinct().Count() == 1)
            {
                break;
            }
        }

        return pots.Sum() + (gen - history.Count) * history[^1];
    }
}