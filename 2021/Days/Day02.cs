using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var (x, y) = (0, 0);

        foreach (var line in this.InputToLines())
        {
            var splits = line.Split(' ');
            var amount = int.Parse(splits[1]);

            switch (splits[0][0])
            {
                case 'f':
                    x += amount;
                    break;
                case 'u':
                    y -= amount;
                    break;
                case 'd':
                    y += amount;
                    break;

            }
        }

        return (x * y).ToString();
    }

    public string SolveSecond()
    {

        var (x, y, aim) = (0, 0, 0);

        foreach (var line in this.InputToLines())
        {
            var splits = line.Split(' ');
            var amount = int.Parse(splits[1]);

            switch (splits[0][0])
            {
                case 'f':
                    x += amount;
                    y += aim * amount;
                    break;
                case 'u':
                    aim -= amount;
                    break;
                case 'd':
                    aim += amount;
                    break;

            }
        }

        return (x * y).ToString();
    }
}