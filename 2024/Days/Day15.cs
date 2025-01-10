using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var grid = input[0].Split("\r\n")[1..^1]
            .Select(s => s[1..^1].ToArray()).ToArray();
        var instructions = input[1].Replace("\r\n", "").ToCharArray();

        var (x, y) = grid.FindFirst('@');
        foreach (var instruction in instructions)
        {
            var (dx, dy) = instruction switch
            {
                '^' => (-1, 0),
                '>' => (0, 1),
                'v' => (1, 0),
                '<' => (0, -1),
                _ => throw new ArgumentOutOfRangeException()
            };

            var n = 1;
            while (grid.IsValidPosition(x + n * dx, y + n * dy) && grid[x + n * dx][y + n * dy] == 'O')
            {
                n++;
            }

            if (!grid.IsValidPosition(x + n * dx, y + n * dy))
            {
                continue;
            }

            if (grid[x + n * dx][y + n * dy] == '#') continue;
            if (n >= 2)
            {
                grid[x + dx][y + dy] = '.';
                grid[x + n * dx][y + n * dy] = 'O';
            }

            grid[x][y] = '.';
            (x, y) = (x + dx, y + dy);
            grid[x][y] = '@';
        }

        var sum = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] == 'O')
                {
                    sum += ((i + 1) * 100) + j + 1;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var grid = input[0].Split("\r\n")[1..^1]
            .Select(s => s[1..^1]
                .Replace("#", "##")
                .Replace("O", "[]")
                .Replace(".", "..")
                .Replace("@", "@.")
                .ToArray()).ToArray();
        var instructions = input[1].Replace("\r\n", "").ToCharArray();

        var (x, y) = grid.FindFirst('@');
        foreach (var instruction in instructions)
        {
            var (dx, dy) = instruction switch
            {
                '^' => (-1, 0),
                '>' => (0, 1),
                'v' => (1, 0),
                '<' => (0, -1),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (dx == 0)
            {
                var n = 1;
                while (grid.IsValidPosition(x, y + n * dy) && grid[x][y + n * dy] is '[' or ']')
                {
                    n++;
                }

                if (!grid.IsValidPosition(x, y + n * dy))
                {
                    continue;
                }

                if (grid[x][y + n * dy] != '.') continue;
                while (n >= 2)
                {
                    grid[x][y + n * dy] = grid[x][y + (n - 1) * dy];
                    n--;
                }

                grid[x][y] = '.';
                y += dy;
                grid[x][y] = '@';
            }
            else
            {
                var items = new HashSet<(int x, int y, char replaceWith)>();
                var toCheck = new HashSet<(int x, int y)>();
                var failed = false;
                if (grid.IsValidPosition(x + dx, y) && grid[x + dx][y] != '#')
                {
                    switch (grid[x + dx][y])
                    {
                        case '[':
                            toCheck.Add((x + dx, y));
                            toCheck.Add((x + dx, y + 1));
                            break;
                        case ']':
                            toCheck.Add((x + dx, y));
                            toCheck.Add((x + dx, y - 1));
                            break;
                    }
                }
                else
                {
                    failed = true;
                }
                while (toCheck.Count > 0 && !failed)
                {
                    var item = toCheck.First();
                    toCheck.Remove(item);
                    if (!grid.IsValidPosition(item.x + dx, item.y))
                    {
                        failed = true;
                        continue;
                    }

                    switch (grid[item.x + dx][item.y])
                    {
                        case '#':
                            failed = true;
                            break;
                        case '.':
                            items.Add((item.x, item.y, grid[item.x - dx][item.y]));
                            items.Add((item.x + dx, item.y, grid[item.x][item.y]));
                            break;
                        case '[':
                            items.Add((item.x, item.y, grid[item.x - dx][item.y]));
                            toCheck.Add((item.x + dx, item.y));
                            toCheck.Add((item.x + dx, item.y + 1));
                            break;
                        case ']':
                            items.Add((item.x, item.y, grid[item.x - dx][item.y]));
                            toCheck.Add((item.x + dx, item.y));
                            toCheck.Add((item.x + dx, item.y - 1));
                            break;
                    }
                }

                if (failed) continue;
                foreach (var item in items)
                {
                    if (!items.Any(i => i.x == item.x - dx && i.y == item.y))
                    {
                        grid[item.x][item.y] = '.';
                    }
                    else
                    {
                        grid[item.x][item.y] = item.replaceWith;
                    }
                }

                grid[x][y] = '.';
                grid[x + dx][y] = '@';
                x += dx;
            }

        }

        var sum = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] == '[')
                {
                    sum += ((i + 1) * 100) + j + 2;
                }
            }
        }

        return sum.ToString();
    }
}