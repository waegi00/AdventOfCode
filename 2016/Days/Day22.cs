using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()[2..]
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x[1..4])
            .Select(x => x.Select(y => int.Parse(y[..^1])).ToList())
            .ToList();

        const int size = 31;
        var nodes = new List<Node>();

        var pos = 0;
        foreach (var line in input)
        {
            var x = pos / size;
            var y = pos % size;
            nodes.Add(new Node(x, y, line[0], line[1], line[2]));

            pos++;
        }

        return nodes.Where(n => n.Used > 0)
            .SelectMany(n => nodes.Where(other => n != other && other.Available >= n.Used))
            .LongCount()
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()[2..]
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x[1..4])
            .Select(x => x.Select(y => int.Parse(y[..^1])).ToList())
            .ToList();

        const int size = 31;
        var nodes = new List<Node>();

        var pos = 0;
        foreach (var line in input)
        {
            var x = pos / size;
            var y = pos % size;
            nodes.Add(new Node(x, y, line[0], line[1], line[2]));

            pos++;
        }

        var xSize = nodes.Max(n => n.X);
        var ySize = nodes.Max(n => n.Y);
        Node? wStart = null, hole = null;
        var grid = new Node[xSize + 1, ySize + 1];

        foreach (var node in nodes)
        {
            grid[node.X, node.Y] = node;
        }

        for (var x = 0; x <= xSize; x++)
        {
            for (var y = 0; y <= ySize; y++)
            {
                var node = grid[x, y];
                if (x == 0 && y == 0) continue;
                if (x == xSize && y == 0) continue;
                if (node.Used == 0)
                {
                    hole = node;
                }
                else if (node.Size > 250)
                {
                    wStart ??= grid[x - 1, y];
                }
            }
        }

        var result = Math.Abs(hole!.X - wStart!.X) + Math.Abs(hole.Y - wStart.Y);
        result += Math.Abs(wStart.X - xSize) + wStart.Y;
        return (result + 5 * (xSize - 1)).ToString();
    }

    private record Node(int X, int Y, int Size, int Used, int Available);
}