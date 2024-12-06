using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2024.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        return GetAccessiblePoints().Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();
        var (iInit, jInit) = input.FindFirst('^');
        var (diInit, djInit) = (-1, 0);

        var sum = 0;
        foreach (var (x, y) in GetAccessiblePoints())
        {
            if (x == iInit && y == jInit)
            {
                continue;
            }
            input[x][y] = '#';

            var (i, j, di, dj) = (iInit, jInit, diInit, djInit);
            var cache = new HashSet<(int, int, int, int)>();
            while (input.IsValidPosition(i, j) && !cache.Contains((i, j, di, dj)))
            {
                cache.Add((i, j, di, dj));

                if (input.IsValidPosition(i + di, j + dj) && input[i + di][j + dj] == '#')
                {
                    (di, dj) = (dj, -di);
                }
                else
                {
                    i += di;
                    j += dj;
                }
            }

            sum += cache.Contains((i, j, di, dj)) ? 1 : 0;

            input[x][y] = '.';
        }

        return sum.ToString();
    }

    private List<(int, int)> GetAccessiblePoints()
    {
        var input = this.InputToLines().ToCharArray();
        var (i, j) = input.FindFirst('^');
        var (di, dj) = (-1, 0);

        while (input.IsValidPosition(i, j))
        {
            input[i][j] = 'X';

            if (input.IsValidPosition(i + di, j + dj) && input[i + di][j + dj] == '#')
            {
                (di, dj) = (dj, -di);
            }

            i += di;
            j += dj;
        }

        return input.FindAll('X').ToList();
    }
}