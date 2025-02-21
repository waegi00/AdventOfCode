using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => Convert.ToInt32(x.Replace('F', '0').Replace('L', '0').Replace('B', '1').Replace('R', '1'), 2))
            .ToArray();

        return input.Max().ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => Convert.ToInt32(x.Replace('F', '0').Replace('L', '0').Replace('B', '1').Replace('R', '1'), 2))
            .Order()
            .ToArray();

        return (input.Select((x, i) => (x, i)).First(x => input[x.i + 1] - x.x != 1).x + 1).ToString();
    }
}