using System.Text;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        for (var i = 0; i < 40; i++)
        {
            var p = 0;
            var n = 1;

            var sb = new StringBuilder();
            while (p < input.Length)
            {
                if (p != input.Length - 1 && input[p] == input[p + 1])
                {
                    n++;
                }
                else
                {
                    sb.Append(n);
                    sb.Append(input[p]);
                    n = 1;
                }

                p++;
            }

            input = sb.ToString();
        }

        return input.Length.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        for (var i = 0; i < 50; i++)
        {
            var p = 0;
            var n = 1;

            var sb = new StringBuilder();
            while (p < input.Length)
            {
                if (p != input.Length - 1 && input[p] == input[p + 1])
                {
                    n++;
                }
                else
                {
                    sb.Append(n);
                    sb.Append(input[p]);
                    n = 1;
                }

                p++;
            }

            input = sb.ToString();
        }

        return input.Length.ToString();
    }
}