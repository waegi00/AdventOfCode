using System.Numerics;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2022.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        return Solve().ToString();
    }

    public string SolveSecond()
    {
        return Solve(true).ToString();
    }

    private int Solve(bool isPart2 = false)
    {
        var grid = this.InputToLines();
        var height = grid.Length - 2;
        var width = grid[0].Trim().Length - 2;

        var directions = new Dictionary<char, Complex>
        {
            ['x'] = 0,
            ['<'] = -1,
            ['>'] = 1,
            ['^'] = -Complex.ImaginaryOne,
            ['v'] = Complex.ImaginaryOne
        };

        var blizzards = new Dictionary<char, HashSet<Complex>>();
        foreach (var d in directions.Keys)
        {
            var set = new HashSet<Complex>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid[y + 1][x + 1] == d)
                    {
                        set.Add(new Complex(x, y));
                    }
                }
            }
            blizzards[d] = set;
        }

        var home = new Complex(0, -1);
        var goal = new Complex(width - 1, height);
        var todo = new List<Complex> { home };
        var time = 0;
        var trip = 0;

        while (todo.Count != 0)
        {
            var newBlizzards = new Dictionary<char, HashSet<Complex>>();
            foreach (var d in directions.Keys)
            {
                newBlizzards[d] = [.. blizzards[d].Select(p => Wrap(p + directions[d]))];
            }
            blizzards = newBlizzards;

            var curr = new HashSet<Complex>();
            foreach (var p in todo)
            {
                foreach (var d in directions.Values)
                {
                    curr.Add(p + d);
                }
            }

            todo = [];
            time++;

            foreach (var pos in curr)
            {
                if ((trip == 0 && pos == goal) || (trip == 1 && pos == home) || (trip == 2 && pos == goal))
                {
                    if (!isPart2 && trip == 0 || trip == 2) return time;
                    todo = [pos];
                    trip++;
                    break;
                }

                if ((blizzards.All(b => !b.Value.Contains(pos)) && Wrap(pos) == pos) || pos == home || pos == goal)
                {
                    todo.Add(pos);
                }
            }
        }

        return -1;

        Complex Wrap(Complex p) => new(p.Real.Mod(width), p.Imaginary.Mod(height));
    }
}