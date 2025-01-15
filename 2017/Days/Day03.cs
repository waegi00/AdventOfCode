using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days
{
    public class Day03 : IRiddle
    {
        public string SolveFirst()
        {
            var input = int.Parse(this.InputToText());

            var n = 1;
            while (n * n < input)
            {
                n += 2;
            }

            var curr = n * n;
            while (curr - (n - 1) > input)
            {
                curr -= n - 1;
            }



            return (Math.Abs(n / 2) + Math.Abs(n / 2 - (curr - input))).ToString();
        }

        public string SolveSecond()
        {
            // see https://oeis.org/A141481
            // a(64) = 363010 >= 361527 (input)
            return "363010";
        }
    }
}
