using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days
{
    public class Day01 : IRiddle
    {
        public string SolveFirst()
        {
            var input = this.InputToText();

            return (input.Count(x => x == '(') - input.Count(x => x == ')')).ToString();
        }

        public string SolveSecond()
        {
            var input = this.InputToText();

            var open = 0;

            for (var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        open++;
                        break;
                    default:
                        if (--open == -1)
                        {
                            return (i + 1).ToString();
                        }
                        break;
                }
            }

            return string.Empty;
        }
    }
}
