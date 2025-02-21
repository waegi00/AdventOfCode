using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day07 : IRiddle
{
    private const string toFind = "shiny gold";

    public string SolveFirst()
    {
        var bags = this.InputToLines()
            .Select(x => x.Split(" bags contain "))
            .ToDictionary(
                x => x[0],
                x => x[1]
                    .Split(',')
                    .Where(y => y != "no other bags.")
                    .Select(y => string.Join(" ", y.Trim().Split(' ').Skip(1).Take(2)))
                    .ToHashSet());

        var cache = new HashSet<string>();

        return bags.Where(x => x.Key != toFind)
            .Count(kvp => ContainsBag(bags, cache, kvp.Key))
            .ToString();
    }

    public string SolveSecond()
    {
        var bags = this.InputToLines()
            .Select(x => x.Split(" bags contain "))
            .ToDictionary(
                x => x[0],
                x => x[1]
                    .Split(',')
                    .Where(y => y != "no other bags.")
                    .Select(y =>
                    {
                        var splits = y.Trim().Split(' ');
                        return (int.Parse(splits[0]), string.Join(" ", splits.Skip(1).Take(2)));
                    })
                    .ToHashSet());

        return BagCount(bags, [], "shiny gold").ToString();
    }

    private static bool ContainsBag(Dictionary<string, HashSet<string>> bags, HashSet<string> cache, string bag)
    {
        if (cache.Contains(bag)) return true;

        var result = bags.ContainsKey(bag) && bags[bag].Any(b => b == toFind || ContainsBag(bags, cache, b));

        if (result) cache.Add(bag);
        return result;
    }

    private static int BagCount(Dictionary<string, HashSet<(int n, string b)>> bags, Dictionary<string, int> cache, string bag)
    {
        if (cache.TryGetValue(bag, out var count)) return count;

        return (bag == toFind ? 0 : 1) + bags[bag].Sum(x => x.n * BagCount(bags, cache, x.b));
    }
}