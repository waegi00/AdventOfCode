using System.Text;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var keypad = new char[][]
        {
            ['1', '2', '3'],
            ['4', '5', '6'],
            ['7', '8', '9']
        };
        var (x, y) = (1, 1);

        var code = new StringBuilder();
        foreach (var line in input)
        {
            foreach (var c in line)
            {
                var (dx, dy) = c switch
                {
                    'U' => (-1, 0),
                    'R' => (0, 1),
                    'D' => (1, 0),
                    'L' => (0, -1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (keypad.IsValidPosition(x + dx, y + dy))
                {
                    (x, y) = (x + dx, y + dy);
                }
            }

            code.Append(keypad[x][y]);
        }

        return code.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var keypad = new char[][]
        {
            ['.', '.', '1', '.', '.'],
            ['.', '2', '3', '4', '.'],
            ['5', '6', '7', '8', '9'],
            ['.', 'A', 'B', 'C', '.'],
            ['.', '.', 'D', '.', '.'],
        };
        var (x, y) = (1, 1);

        var code = new StringBuilder();
        foreach (var line in input)
        {
            foreach (var c in line)
            {
                var (dx, dy) = c switch
                {
                    'U' => (-1, 0),
                    'R' => (0, 1),
                    'D' => (1, 0),
                    'L' => (0, -1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (keypad.IsValidPosition(x + dx, y + dy) && keypad[x + dx][y + dy] != '.')
                {
                    (x, y) = (x + dx, y + dy);
                }
            }

            code.Append(keypad[x][y]);
        }

        return code.ToString();
    }
}