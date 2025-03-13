using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2021.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines().ToCharArray();

        var i = 0;
        while (true)
        {
            var moves = 0;

            grid.FindAll('>')
                .Where(x => grid[x.i][(x.j + 1) % grid[x.i].Length] == '.')
                .ToList()
                .ForEach(x =>
                {
                    grid[x.i][x.j] = '.';
                    grid[x.i][(x.j + 1) % grid[x.i].Length] = '>';
                    moves++;
                });

            grid.FindAll('v')
                .Where(x => grid[(x.i + 1) % grid.Length][x.j] == '.')
                .ToList()
                .ForEach(x =>
                {
                    grid[x.i][x.j] = '.';
                    grid[(x.i + 1) % grid.Length][x.j] = 'v';
                    moves++;
                });

            i++;

            if (moves == 0) return i.ToString();
        }
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }
}