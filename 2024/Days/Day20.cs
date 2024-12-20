using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day20 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .Select(l => l.Select(c => c).ToArray())
            .ToArray();

        var start = grid.FindFirst('S');
        var end = grid.FindFirst('E');
        var curr = start;

        var i = 0;
        var path = new Dictionary<(int x, int y), int> { { curr, i } };
        while (curr != end)
        {
            curr = grid.Neighbours(curr.i, curr.j, true, true, false).First(n => !path.ContainsKey(n.Item2) && grid[n.Item2.Item1][n.Item2.Item2] != '#').Item2;
            path.Add(curr, ++i);
        }

        var result = new List<int>();
        var pairs = path.Pairs();
        foreach (var (((x1, y1), n1), ((x2, y2), n2)) in pairs)
        {
            if (x1 == x2 && Math.Abs(y1 - y2) == 2 && !path.ContainsKey((x1, (y1 + y2) / 2)) ||
                y1 == y2 && Math.Abs(x1 - x2) == 2 && !path.ContainsKey(((x1 + x2) / 2, y1)))
            {
                result.Add(Math.Abs(n1 - n2) - 2);
            }
        }

        return result.Count(x => x >= 100).ToString();
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .Select(l => l.Select(c => c).ToArray())
            .ToArray();

        var start = grid.FindFirst('S');
        var end = grid.FindFirst('E');
        var curr = start;

        var i = 0;
        var path = new Dictionary<(int x, int y), int> { { curr, i } };
        while (curr != end)
        {
            curr = grid.Neighbours(curr.i, curr.j, true, true, false).First(n => !path.ContainsKey(n.Item2) && grid[n.Item2.Item1][n.Item2.Item2] != '#').Item2;
            path.Add(curr, ++i);
        }

        var result = new List<int>();
        var pairs = path.Pairs();
        foreach (var (((x1, y1), n1), ((x2, y2), n2)) in pairs)
        {
            if (Math.Abs(x1 - x2) + Math.Abs(y1 - y2) <= 20)
            {
                result.Add(Math.Abs(n1 - n2) - Math.Abs(x1 - x2) - Math.Abs(y1 - y2));
            }
        }

        return result.Count(x => x >= 100).ToString();
    }
}