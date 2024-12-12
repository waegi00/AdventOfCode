using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Numerics;

namespace AdventOfCode._2015.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        var numbers = this.InputToLines().Select(int.Parse).OrderByDescending(x => x).ToList();
        var groupSize = numbers.Sum() / 3;
        for (var i = 1; i <= numbers.Count; i++)
        {
            var combinations = CombinationsHelper(numbers, i)
                .Where(c => c.Sum() == groupSize)
                .Select(c => c.Aggregate(BigInteger.One, (prod, item) => prod * item))
                .ToList();

            if (combinations.Count > 0)
            {
                return combinations.Min().ToString();
            }
        }

        return "";
    }

    public string SolveSecond()
    {
        var numbers = this.InputToLines().Select(int.Parse).OrderByDescending(x => x).ToList();
        var groupSize = numbers.Sum() / 4;
        for (var i = 1; i <= numbers.Count; i++)
        {
            var combinations = CombinationsHelper(numbers, i)
                .Where(c => c.Sum() == groupSize)
                .Select(c => c.Aggregate(BigInteger.One, (prod, item) => prod * item))
                .ToList();

            if (combinations.Count > 0)
            {
                return combinations.Min().ToString();
            }
        }

        return "";
    }

    private static IEnumerable<List<int>> CombinationsHelper(List<int> list, int length, int startIndex = 0, List<int>? currentCombination = null)
    {
        currentCombination ??= [];
        if (currentCombination.Count == length)
        {
            yield return [.. currentCombination];
            yield break;
        }

        for (var i = startIndex; i < list.Count; i++)
        {
            currentCombination.Add(list[i]);
            foreach (var combination in CombinationsHelper(list, length, i + 1, currentCombination))
            {
                yield return combination;
            }

            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }
}