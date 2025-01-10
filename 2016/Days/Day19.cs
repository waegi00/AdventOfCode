using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var n = int.Parse(this.InputToText());
        var result = 0;

        for (var i = 2; i <= n; i++)
        {
            result = (result + 2) % i;
        }

        return (result + 1).ToString();
    }

    public string SolveSecond()
    {
        var n = int.Parse(this.InputToText());
        var result = 1;

        for (var i = 1; i < n; i++)
        {
            result = result % i + 1;
            if (result > (i + 1) / 2)
            {
                result++;
            }
        }
        return result.ToString();
    }
}