using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" -> "))
            .ToList();

        var tree = new Dictionary<string, string>();
        foreach (var line in input)
        {
            var parent = line[0].Split(' ')[0];
            var children = line.Length > 1
                ? line[^1].Split(", ")
                : [];

            foreach (var child in children)
            {
                tree.Add(child, parent);
            }
        }

        return Parent(tree);
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" -> "))
            .ToList();

        var tree = new Dictionary<string, (int, string[])>();
        foreach (var line in input)
        {
            var splits = line[0].Split(' ');
            var name = splits[0];
            var weight = int.Parse(splits[1][1..^1]);

            var children = line.Length > 1
                ? line[^1].Split(", ")
                : [];

            tree.Add(name, (weight, children));
        }

        var parent = SolveFirst();

        return IsBalanced(tree, parent).Item2.ToString();
    }

    private static string Parent(Dictionary<string, string> d)
    {
        var curr = d.Keys.First();

        while (d.ContainsKey(curr))
        {
            curr = d[curr];
        }

        return curr;
    }

    private static (bool, int) IsBalanced(Dictionary<string, (int w, string[] c)> d, string curr)
    {
        if (d[curr].c.Length == 0)
        {
            return (true, d[curr].w);
        }

        var res = d[curr].c.Select(x => IsBalanced(d, x)).ToList();

        if (res.All(x => x.Item1))
        {
            if (res.DistinctBy(x => x.Item2).Count() == 1)
            {
                return (true, d[curr].w + res.Sum(x => x.Item2));
            }

            var grouping = res.GroupBy(x => x.Item2).ToList();
            var offWeight = grouping.First(x => x.Count() == 1).First().Item2;
            var correctWeight = grouping.First(x => x.Count() != 1).First().Item2;
            var i = res.Select((x, i) => (x, i)).First(x => x.x.Item2 == offWeight).i;

            return (false, d[d[curr].c[i]].w + (correctWeight - offWeight));
        }

        return res.First(x => !x.Item1);
    }
}