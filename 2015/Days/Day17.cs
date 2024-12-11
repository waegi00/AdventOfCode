using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var containers = this.InputToLines().Select(int.Parse).ToList();

        return Combinations(containers, 150)
            .Count
            .ToString();
    }

    public string SolveSecond()
    {
        var containers = this.InputToLines().Select(int.Parse).ToList();
        var combinations = Combinations(containers, 150);
        return combinations
            .Count(x => x.Count == combinations.Min(c => c.Count))
            .ToString();
    }

    private static List<List<int>> Combinations(List<int> containers, int targetVolume)
    {
        var combinations = new List<List<int>>();
        var stack = new Stack<(int currentIndex, int remainingVolume, List<int> currentCombination)>();
        stack.Push((0, targetVolume, []));

        while (stack.Count > 0)
        {
            var (currentIndex, remainingVolume, currentCombination) = stack.Pop();

            if (remainingVolume == 0)
            {
                combinations.Add([..currentCombination]);
                continue;
            }

            if (remainingVolume < 0 || currentIndex >= containers.Count)
            {
                continue;
            }

            var includeCombination = new List<int>(currentCombination) { containers[currentIndex] };
            stack.Push((currentIndex + 1, remainingVolume - containers[currentIndex], includeCombination));
            stack.Push((currentIndex + 1, remainingVolume, currentCombination));
        }

        return combinations;
    }
}