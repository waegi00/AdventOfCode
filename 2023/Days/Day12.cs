using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day12.txt");

        var sum = input
            .Select(line => line.Split(' '))
            .Select(splits =>
                CalculatePossibilities(
                    splits[0].Trim(),
                    splits[1].Trim().Split(",").Select(int.Parse)))
            .Sum();

        return sum.ToString();
    }

    public string SolveSecond()
    {
        return "";
    }

    private static int CalculatePossibilities(string input, IEnumerable<int> numbers, int repeat = 0)
    {
        var text = input;
        var numbersList = numbers.Reverse().ToList();
        var stack = new Stack<int>(numbersList);

        for (var i = 0; i < repeat; i++)
        {
            text += "?" + input;

            foreach (var item in numbersList)
            {
                stack.Push(item);
            }
        }

        return Solve(text.ToCharArray(), stack);
    }

    private static int Solve(char[] input, Stack<int> numbers)
    {
        return Solve(input, numbers, numbers.Pop(), true);
    }

    private static int Solve(char[] input, Stack<int> numbers, int remaining, bool fromPoint = false)
    {
        var c = input[0];

        if (input.Length == 1)
        {
            return !numbers.Any() && (remaining == 0 && c is '?' or '.' || remaining == 1 && c is '?' or '#') ? 1 : 0;
        }

        switch (c)
        {
            case '#':
                if (remaining == 0)
                {
                    return 0;
                }

                var newStack1 = numbers.CloneStack();
                return Solve(input[1..], newStack1, remaining - 1);

            case '.':
                if (fromPoint)
                {
                    var newStack2 = numbers.CloneStack();
                    return Solve(input[1..], newStack2, remaining, true);
                }

                if (remaining > 0)
                {
                    return 0;
                }

                if (!numbers.Any())
                {
                    var newStack3 = numbers.CloneStack();
                    return Solve(input[1..], newStack3, remaining, true);
                }

                var newStack4 = numbers.CloneStack();
                return Solve(input[1..], newStack4, newStack4.Pop(), true);
            case '?':
                var a = 0;

                if (remaining > 0)
                {
                    var newStack5 = numbers.CloneStack();
                    a = Solve(input[1..], newStack5, remaining - 1);
                }

                if (fromPoint)
                {
                    var newStack6 = numbers.CloneStack();
                    return a + Solve(input[1..], newStack6, remaining, true);
                }

                if (remaining > 0)
                {
                    return a;
                }

                if (!numbers.Any())
                {
                    var newStack7 = numbers.CloneStack();
                    return Solve(input[1..], newStack7, remaining, true);
                }

                var newStack8 = numbers.CloneStack();
                return Solve(input[1..], newStack8, newStack8.Pop(), true);
        }

        throw new Exception("Should not happen");
    }

    private static bool IsPossbile(string input, IEnumerable<int> numbers)
    {
        return input.Split(".").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Length).SequenceEqual(numbers);
    }
}


public static class StackExtensions
{
    public static Stack<T> CloneStack<T>(this Stack<T> original)
    {
        var arr = new T[original.Count];
        original.CopyTo(arr, 0);
        Array.Reverse(arr);
        return new Stack<T>(arr);
    }
}