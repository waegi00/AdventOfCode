using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => (x[0], int.Parse(x[1..])))
            .ToArray();

        var (north, east) = (0, 0);
        var (dn, de) = (0, 1);

        foreach (var (c, x) in input)
        {
            switch (c)
            {
                case 'N':
                    north += x;
                    break;
                case 'S':
                    north -= x;
                    break;
                case 'E':
                    east += x;
                    break;
                case 'W':
                    east -= x;
                    break;
                case 'L':
                    for (var i = 0; i < x; i += 90)
                    {
                        (dn, de) = RotateLeft((dn, de));
                    }
                    break;
                case 'R':
                    for (var i = 0; i < x; i += 90)
                    {
                        (dn, de) = RotateRight((dn, de));
                    }
                    break;
                case 'F':
                    (north, east) = (north + dn * x, east + de * x);
                    break;
            }
        }

        return (Math.Abs(north) + Math.Abs(east)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => (x[0], int.Parse(x[1..])))
            .ToArray();

        var (north, east) = (0, 0);
        var (wNorth, wEast) = (1, 10);

        foreach (var (c, x) in input)
        {
            switch (c)
            {
                case 'N':
                    wNorth += x;
                    break;
                case 'S':
                    wNorth -= x;
                    break;
                case 'E':
                    wEast += x;
                    break;
                case 'W':
                    wEast -= x;
                    break;
                case 'L':
                    for (var i = 0; i < x; i += 90)
                    {
                        var nNorth = north + wEast - east;
                        var nEast = east - wNorth + north;
                        (wNorth, wEast) = (nNorth, nEast);
                    }
                    break;
                case 'R':
                    for (var i = 0; i < x; i += 90)
                    {
                        var nNorth = north - wEast + east;
                        var nEast = east + wNorth - north;
                        (wNorth, wEast) = (nNorth, nEast);
                    }
                    break;
                case 'F':
                    var (dx, dy) = ((wNorth - north) * x, (wEast - east) * x);
                    north += dx;
                    wNorth += dx;
                    east += dy;
                    wEast += dy;
                    break;
            }
        }

        return (Math.Abs(north) + Math.Abs(east)).ToString();
    }

    private static (int, int) RotateRight((int x, int y) curr)
    {
        return (-curr.y, curr.x);
    }

    private static (int, int) RotateLeft((int x, int y) curr)
    {
        return (curr.y, -curr.x);
    }
}