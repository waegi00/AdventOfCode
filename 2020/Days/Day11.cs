using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2020.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .ToCharArray();

        var seen = new HashSet<char[][]>(new JaggedArrayComparer<char>());

        while (seen.Add(input))
        {
            input = input
                .Select((row, i) => row
                    .Select((value, j) => Value(value, input.Neighbours(i, j).Select(x => x.Item1)))
                    .ToArray())
                .ToArray();
        }

        return input.Sum(r => r.Count(v => v == '#')).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .ToCharArray();

        var seen = new HashSet<char[][]>(new JaggedArrayComparer<char>());

        while (seen.Add(input))
        {
            input = input
                .Select((row, i) => row
                    .Select((value, j) => Value2(value, Neighbours(input, i, j)))
                    .ToArray())
                .ToArray();
        }

        return input.Sum(r => r.Count(v => v == '#')).ToString();
    }

    private static char Value(char curr, IEnumerable<char> neighbours)
    {
        switch (curr)
        {
            case 'L':
                if (neighbours.All(n => n != '#')) return '#';
                break;
            case '#':
                if (neighbours.Count(n => n == '#') >= 4) return 'L';
                break;
        }

        return curr;
    }

    private static char Value2(char curr, IEnumerable<char> neighbours)
    {
        switch (curr)
        {
            case 'L':
                if (neighbours.All(n => n != '#')) return '#';
                break;
            case '#':
                if (neighbours.Count(n => n == '#') >= 5) return 'L';
                break;
        }

        return curr;
    }

    private static IEnumerable<char> Neighbours(char[][] grid, int i, int j)
    {
        foreach (var (dx, dy) in new[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) })
        {
            var x = i + dx;
            var y = j + dy;

            while (grid.IsValidPosition(x, y) && grid[x][y] == '.')
            {
                x += dx;
                y += dy;
            }

            if (grid.IsValidPosition(x, y))
            {
                yield return grid[x][y];
            }
        }
    }
}