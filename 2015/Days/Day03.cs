using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days
{
    public class Day03 : IRiddle
    {
        public string SolveFirst()
        {
            var input = this.InputToText();

            var dict = new HashSet<(int, int)>();

            var i = 0;
            var j = 0;

            foreach (var c in input)
            {
                switch (c)
                {
                    case '^':
                        i--;
                        break;
                    case '>':
                        j++;
                        break;
                    case 'v':
                        i++;
                        break;
                    case '<':
                        j--;
                        break;
                }

                dict.Add((i, j));
            }


            return dict.Count.ToString();
        }

        public string SolveSecond()
        {
            var input = this.InputToText();

            var dict = new HashSet<(int, int)>();

            var positions = new[] { (0, 0), (0, 0) };

            for (var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '^':
                        positions[i % 2].Item1--;
                        break;
                    case '>':
                        positions[i % 2].Item2++;
                        break;
                    case 'v':
                        positions[i % 2].Item1++;
                        break;
                    case '<':
                        positions[i % 2].Item2--;
                        break;
                }

                dict.Add(positions[i % 2]);
            }


            return dict.Count.ToString();
        }
    }
}
