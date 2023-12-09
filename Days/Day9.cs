using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day9 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day9.txt");

        var sum = 0;

        foreach (var line in input)
        {
            var h = new History { Nums = line.Split(" ").Select(x => int.Parse(x.Trim())).ToList() };
            sum += h.CalculateNextNum(h.Nums);
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day9.txt");

        var sum = 0;

        foreach (var line in input)
        {
            var h = new History { Nums = line.Split(" ").Select(x => int.Parse(x.Trim())).ToList() };
            sum += h.CalculatePreviousNum(h.Nums);
        }

        return sum.ToString();
    }

    private class History
    {
        public List<int> Nums { get; set; } = new List<int>();

        public int CalculateNextNum(List<int> nums)
        {
            if (nums.All(x => x == 0))
            {
                return 0;
            }

            var newNums = new List<int>();

            for (int i = 0; i < nums.Count - 1; i++)
            {
                newNums.Add(nums[i + 1] - nums[i]);
            }

            return nums.Last() + CalculateNextNum(newNums);
        }

        public int CalculatePreviousNum(List<int> nums)
        {
            if (nums.All(x => x == 0))
            {
                return 0;
            }

            var newNums = new List<int>();

            for (int i = 0; i < nums.Count - 1; i++)
            {
                newNums.Add(nums[i + 1] - nums[i]);
            }

            return nums.First() - CalculatePreviousNum(newNums);
        }
    }
}
