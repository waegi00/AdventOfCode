using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var step = int.Parse(this.InputToText());

        var spinlock = new List<int> { 0 };
        var pos = 0;

        for (var i = 1; i <= 2017; i++)
        {
            var next = (pos + step) % spinlock.Count;
            spinlock.Insert(next + 1, i);
            pos = next + 1;
        }


        return spinlock[(pos + 1) % spinlock.Count].ToString();
    }

    public string SolveSecond()
    {
        var step = int.Parse(this.InputToText());

        var spinlock = new List<int> { 0 };
        var pos = 0;
        var res = -1;

        for (var i = 1; i <= 50000000; i++)
        {
            pos = (pos + step) % i + 1;
            if (pos == 1)
            {
                res = i;
            }
        }


        return res.ToString();
    }
}