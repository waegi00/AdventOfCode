using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2015.Days;

public class Day20 : IRiddle
{
    public string SolveFirst()
    {
        var num = long.Parse(this.InputToText());

        var houseNumber = 1;
        while (true)
        {
            if (PrimeHelper.Divisors(houseNumber).Sum() * 10 >= num)
            {
                break;
            }
            houseNumber++;
        }

        return houseNumber.ToString();
    }

    public string SolveSecond()
    {
        var num = long.Parse(this.InputToText());

        var houseNumber = 1;
        while (true)
        {
            if (PrimeHelper.Divisors(houseNumber).Where(d => houseNumber / d <= 50).Sum() * 11 >= num)
            {
                break;
            }
            houseNumber++;
        }

        return houseNumber.ToString();
    }
}