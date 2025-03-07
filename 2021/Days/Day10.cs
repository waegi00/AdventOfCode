using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2021.Days;

public class Day10 : IRiddle
{
    private static readonly HashSet<char> opening = ['(', '[', '{', '<'];
    private static readonly Dictionary<char, char> brackets = new() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' }, };
    private static readonly Dictionary<char, int> penalties = new() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 }, };
    private static readonly Dictionary<char, int> fillCosts = new() { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 }, };

    public string SolveFirst()
    {
        return this.InputToLines()
            .Sum(Penalty)
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Select(Fill)
            .Where(x => x != 0)
            .ToArray()
            .Median()
            .ToString();
    }

    private static int Penalty(string line)
    {
        var stack = new Stack<char>();

        foreach (var c in line)
        {
            if (opening.Contains(c))
            {
                stack.Push(c);
            }
            else
            {
                var prev = stack.Pop();
                if (brackets[prev] != c)
                {
                    return penalties[c];
                }
            }
        }
        return 0;
    }

    private static long Fill(string line)
    {
        var stack = new Stack<char>();

        foreach (var c in line)
        {
            if (opening.Contains(c))
            {
                stack.Push(c);
            }
            else
            {
                var prev = stack.Pop();
                if (brackets[prev] != c)
                {
                    return 0;
                }
            }
        }

        var sum = 0L;
        while (stack.TryPop(out var item))
        {
            sum *= 5;
            sum += fillCosts[brackets[item]];
        }
        return sum;
    }
}