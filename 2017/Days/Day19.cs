using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2017.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines().ToCharArray();

        (int, int)? prev = null;
        var (x, y) = grid.FindFirst('|');
        var (dx, dy) = (1, 0);
        var visited = new HashSet<(int, int)>();

        var result = string.Empty;
        while (grid.IsValidPosition(x, y) && grid[x][y] != ' ')
        {
            if (prev == null ||
                grid[x][y].IsUpper() ||
                grid[x][y] == '+' ||
                dy == 0 && grid[x][y] == '|' ||
                dx == 0 && grid[x][y] == '-')
            {
                if (!visited.Add((x, y)))
                {
                    break;
                }
            }

            var c = grid[x][y];
            if (c.IsUpper())
            {
                result += c;
            }
            else if (c == '+')
            {
                var ns = grid.Neighbours(x, y, includeDiagonal: false)
                    .Where(n => n.Item2 != prev && n.Item1 != ' ')
                    .ToList();

                if (ns.Count > 0)
                {
                    var (_, (nx, ny)) = ns.First();
                    (dx, dy) = (nx - x, ny - y);
                }
            }

            prev = (x, y);
            (x, y) = (x + dx, y + dy);
        }

        return result;
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines().ToCharArray();

        var steps = 0;
        (int, int)? prev = null;
        var (x, y) = grid.FindFirst('|');
        var (dx, dy) = (1, 0);
        var visited = new HashSet<(int, int)>();

        while (grid.IsValidPosition(x, y) && grid[x][y] != ' ')
        {
            if (prev == null ||
                grid[x][y].IsUpper() ||
                grid[x][y] == '+' ||
                dy == 0 && grid[x][y] == '|' ||
                dx == 0 && grid[x][y] == '-')
            {
                if (!visited.Add((x, y)))
                {
                    break;
                }
            }

            var c = grid[x][y];
            if (c == '+')
            {
                var ns = grid.Neighbours(x, y, includeDiagonal: false)
                    .Where(n => n.Item2 != prev && n.Item1 != ' ')
                    .ToList();

                if (ns.Count > 0)
                {
                    var (_, (nx, ny)) = ns.First();
                    (dx, dy) = (nx - x, ny - y);
                }
            }

            steps++;

            prev = (x, y);
            (x, y) = (x + dx, y + dy);
        }

        return steps.ToString();
    }
}