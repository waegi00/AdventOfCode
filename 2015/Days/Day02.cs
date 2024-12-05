using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Geometry;

namespace AdventOfCode._2015.Days
{
    public class Day02 : IRiddle
    {
        public string SolveFirst()
        {
            var input = this.InputToLines();

            var sum = 0;

            foreach (var line in input)
            {
                var nums = line.Split('x').Select(int.Parse).ToList();

                var cuboid = new Cuboid<int>(nums[0], nums[1], nums[2]);
                sum += cuboid.Surface();

                var (a, b, c) = cuboid.Sides();
                int[] slack = [a, b, c];
                sum += slack.Min();
            }

            return sum.ToString();
        }

        public string SolveSecond()
        {
            var input = this.InputToLines();

            var sum = 0;

            foreach (var line in input)
            {
                var nums = line.Split('x').Select(int.Parse).ToList();

                var l = 0;
                l += nums.Aggregate(1, (acc, num) => acc * num);
                l += 2 * nums.Sum();
                l -= 2 * nums.Max();

                sum += l;
            }

            return sum.ToString();
        }
    }
}
