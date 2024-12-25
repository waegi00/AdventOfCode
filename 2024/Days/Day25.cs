using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"));

        var keys = new List<List<int>>();
        var locks = new List<List<int>>();

        foreach (var item in input)
        {
            var heights = new List<int>();

            for (var i = 0; i < item[0].Length; i++)
            {
                heights.Add(item.Count(x => x[i] == '#') - 1);
            }

            if (item[0].All(x => x == '.'))
            {
                keys.Add(heights);
            }
            else
            {
                locks.Add(heights);
            }
        }

        return locks.Sum(l => 
            keys.Count(k => 
                l.Select((x, i) => (x, i))
                    .All(x => x.x + k[x.i] < 6)))
            .ToString();
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }
}