using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines()
            .Select(x => x.Split("\t").Select(int.Parse).Order().ToList())
            .Sum(x => x[^1] - x[0])
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Select(x => x.Split("\t").Select(int.Parse).OrderDescending().ToList())
            .Sum(Div)
            .ToString();
    }

    private static int Div(List<int> numbers)
    {
        for (var i = 0; i < numbers.Count; i++)
        {
            for (var j = i + 1; j < numbers.Count; j++)
            {
                if (numbers[i] % numbers[j] == 0)
                {
                    return numbers[i] / numbers[j];
                }
            }
        }

        throw new Exception();
    }
}