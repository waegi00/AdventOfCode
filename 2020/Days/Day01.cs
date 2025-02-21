using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        return SumOfK(this.InputToLines().Select(int.Parse).ToHashSet(), 2020, 2)
            .Product()
            .ToString();
    }

    public string SolveSecond()
    {
        return SumOfK(this.InputToLines().Select(int.Parse).ToHashSet(), 2020, 3)
            .Product()
            .ToString();
    }

    private static IEnumerable<int> SumOfK(HashSet<int> numbers, int goal, int k)
    {
        if (k == 1)
        {
            return numbers.Contains(goal) ? [goal] : [];
        }

        foreach (var n in numbers)
        {
            var ns = SumOfK(numbers, goal - n, k - 1).ToList();
            if (ns.Count == 0) continue;
            return ns.Append(n);
        }

        return [];
    }
}