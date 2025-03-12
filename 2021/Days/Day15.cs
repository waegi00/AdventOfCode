using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        return ShortestPath(input).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

        var boosted = Enumerable.Range(0, input.Length * 5)
            .Select(_ => new int[input[0].Length * 5])
            .ToArray();

        for (var di = 0; di < 5; di++)
        {
            for (var dj = 0; dj < 5; dj++)
            {
                for (var i = 0; i < input.Length; i++)
                {
                    for (var j = 0; j < input[i].Length; j++)
                    {
                        boosted[di * input.Length + i][dj * input[0].Length + j] = (input[i][j] + di + dj - 1) % 9 + 1;
                    }
                }
            }
        }

        return ShortestPath(boosted).ToString();
    }

    private static long ShortestPath(int[][] input)
    {
        var end = (input.Length - 1, input[0].Length - 1);

        var seen = new HashSet<(int x, int y)>();
        var toVisit = new PriorityQueue<(int x, int y), int>();
        toVisit.Enqueue((0, 0), 0);

        while (toVisit.TryDequeue(out var item, out var priority))
        {
            if (item == end)
            {
                return priority;
            }

            if (!seen.Add(item)) continue;

            foreach (var (n, (nx, ny)) in input.Neighbours(item.x, item.y, includeDiagonal: false))
            {
                toVisit.Enqueue((nx, ny), priority + n);
            }
        }

        return -1;
    }
}