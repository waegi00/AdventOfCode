using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        var range = this.InputToText()
            .Split('-')
            .Select(int.Parse)
            .ToArray();

        var sum = 0;
        for (var i = range[0]; i < range[1]; i++)
        {
            var s = i.ToString();

            var j = 0;
            var duplicate = false;
            while (j < s.Length - 1 && s[j] <= s[j + 1])
            {
                if (s[j] == s[j + 1])
                {
                    duplicate = true;
                }
                j++;
            }

            if (j < s.Length - 1 || !duplicate) continue;

            sum++;
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var range = this.InputToText()
            .Split('-')
            .Select(int.Parse)
            .ToArray();

        var sum = 0;
        for (var i = range[0]; i < range[1]; i++)
        {
            var s = i.ToString();

            var j = 0;
            var duplicate = false;
            while (j < s.Length - 1 && s[j] <= s[j + 1])
            {
                if (s[j] == s[j + 1])
                {
                    if ((j == 0 || s[j - 1] != s[j]) &&
                        (j >= s.Length - 2 || s[j + 2] != s[j + 1]))
                    {
                        duplicate = true;
                    }
                }
                j++;
            }

            if (j < s.Length - 1 || !duplicate) continue;

            sum++;
        }

        return sum.ToString();
    }
}