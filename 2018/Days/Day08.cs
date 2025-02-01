using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        return Parse(input).total.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        return Parse(input).score.ToString();
    }

    private static (int total, int score, List<int> data) Parse(List<int> data)
    {
        var children = data[0];
        var metas = data[1];
        var scores = new List<int>();
        data = data[2..];
        var totals = 0;

        for (var i = 0; i < children; i++)
        {
            var res = Parse(data);
            totals += res.total;
            data = res.data;
            scores.Add(res.score);
        }

        totals += data[..metas].Sum();

        return (
            totals,
            children == 0
                ? data[..metas].Sum()
                : data.Take(metas).Where(k => k > 0 && k <= scores.Count).Sum(k => scores[k - 1]), 
            data[metas..]
        );
    }
}