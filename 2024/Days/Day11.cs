using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        return GetStones(this.InputToText(), 25).ToString();
    }

    public string SolveSecond()
    {
        return GetStones(this.InputToText(), 75).ToString();
    }

    private static long GetStones(string input, int iterations)
    {
        var nums = input.Split(" ").Select(long.Parse).ToDictionary(n => n, _ => 1L);

        for (var i = 0; i < iterations; i++)
        {
            var next = new Dictionary<long, long>();

            foreach (var num in nums.Keys)
            {
                foreach (var n in Blink(num).Where(x => !next.TryAdd(x, nums[num])))
                {
                    next[n] += nums[num];
                }
            }

            nums = next;
        }

        return nums.Values.Sum();
    }

    private static List<long> Blink(long number)
    {
        return number.ToString() switch
        {
            "0" => [1],
            var str when str.Length % 2 == 0 => 
            [
                long.Parse(str[..(str.Length / 2)]), long.Parse(str[(str.Length / 2)..])
            ],
            _ => [number * 2024]
        };
    }
}