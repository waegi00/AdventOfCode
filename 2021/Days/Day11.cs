using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        var flashes = 0;

        for (var i = 0; i < 100; i++)
        {
            input = input.Select(x => x
                    .Select(y => (y + 1) % 10)
                    .ToArray())
                .ToArray();

            var flashed = new Stack<(int x, int y)>(input.FindAll(0));

            while (flashed.TryPop(out var item))
            {
                foreach (var (_, (nx, ny)) in input.Neighbours(item.x, item.y))
                {
                    if (input[nx][ny] == 0) continue;

                    input[nx][ny] = (input[nx][ny] + 1) % 10;
                    if (input[nx][ny] == 0)
                    {
                        flashed.Push((nx, ny));
                    }
                }
            }

            flashes += input.Sum(x => x.Count(y => y == 0));
        }

        return flashes.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        var i = 0;

        while (true)
        {
            input = input.Select(x => x
                    .Select(y => (y + 1) % 10)
                    .ToArray())
                .ToArray();

            var flashed = new Stack<(int x, int y)>(input.FindAll(0));

            while (flashed.TryPop(out var item))
            {
                foreach (var (_, (nx, ny)) in input.Neighbours(item.x, item.y))
                {
                    if (input[nx][ny] == 0) continue;

                    input[nx][ny] = (input[nx][ny] + 1) % 10;
                    if (input[nx][ny] == 0)
                    {
                        flashed.Push((nx, ny));
                    }
                }
            }

            if (input.All(x => x.All(y => y == 0)))
            {
                return (i + 1).ToString();
            }

            i++;
        }
    }
}