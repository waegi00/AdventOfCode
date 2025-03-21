using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2022.Days;

public class Day25 : IRiddle
{
    private const string digits = "=-012";
    private const int @base = 5;
    private const int offset = 2;

    public string SolveFirst()
    {
        return ToSnafu(this.InputToLines().Sum(FromSnafu));
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }

    private static long FromSnafu(string snafu)
    {
        return new string(snafu.Reverse().ToArray())
            .WithIndex()
            .Sum(x => (long)Math.Pow(@base, x.index) * (digits.IndexOf(x.item) - offset));
    }

    private static string ToSnafu(long n)
    {
        var result = string.Empty;

        while (n != 0)
        {
            var index = (n + offset).Mod(@base);
            result += digits[(int)index];
            if (index < offset)
            {
                n += @base;
            }

            n /= @base;
        }

        return new string(result.Reverse().ToArray());
    }
}