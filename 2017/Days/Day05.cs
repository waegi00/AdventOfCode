using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(int.Parse)
            .ToList();

        var curr = 0;
        var n = 0;

        while (curr >= 0 && curr < input.Count)
        {
            var tmp = curr;
            curr += input[curr];
            input[tmp]++;
            n++;
        }

        return n.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(int.Parse)
            .ToList();

        var curr = 0;
        var n = 0;

        while (curr >= 0 && curr < input.Count)
        {
            var tmp = curr;
            curr += input[curr];
            if (input[tmp] >= 3)
            {
                input[tmp]--;
            }
            else
            {
                input[tmp]++;
            }
            n++;
        }

        return n.ToString();
    }
}