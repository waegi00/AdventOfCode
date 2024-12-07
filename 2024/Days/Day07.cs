using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var sum = 0L;

        foreach (var line in input)
        {
            var splits = line.Split(" ");
            var res = long.Parse(splits[0][..^1]);
            var nums = splits[1..].Select(long.Parse).ToList();
            if (IsMatch(res, nums[1..], nums[0]))
            {
                sum += res;
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var sum = 0L;

        foreach (var line in input)
        {
            var splits = line.Split(" ");
            var res = long.Parse(splits[0][..^1]);
            var nums = splits[1..].Select(long.Parse).ToList();
            if (IsMatch2(res, nums[1..], nums[0]))
            {
                sum += res;
            }
        }

        return sum.ToString();
    }

    private static bool IsMatch(long res, List<long> numbers, long curr)
    {
        if (numbers.Count == 1)
        {
            return res == curr + numbers[0] || res == curr * numbers[0];
        }
        else
        {
            var n = numbers[0];
            numbers.RemoveAt(0);
            return IsMatch(res, [.. numbers], curr * n) ||
                   IsMatch(res, [.. numbers], curr + n);
        }
    }

    private static bool IsMatch2(long res, List<long> numbers, long curr)
    {
        if (numbers.Count == 1)
        {
            return res == curr + numbers[0] || res == curr * numbers[0] || res == long.Parse(curr + "" + numbers[0]);
        }

        var n = numbers[0];
        numbers.RemoveAt(0);
        return IsMatch2(res, [.. numbers], curr * n) ||
               IsMatch2(res, [.. numbers], curr + n) ||
               IsMatch2(res, [.. numbers], long.Parse(curr + "" + n));
    }
}