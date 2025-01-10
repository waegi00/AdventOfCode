using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Graph;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2016.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().ToCharArray();

        var maxN = input.SelectMany(x => x).Where(x => x.IsNumeric()).Max() - '0';
        var graph = new Graph<int>();

        for (var i = 0; i < maxN; i++)
        {
            var remaining = maxN - i;
            var start = input.FindFirst((char)('0' + i));
            input[start.i][start.j] = '.';

            var visited = new Dictionary<(int, int), int>();
            var toVisit = new Queue<(int x, int y, int n)>();
            toVisit.Enqueue((start.i, start.j, 0));

            while (toVisit.TryDequeue(out var curr))
            {
                if (!visited.TryAdd((curr.x, curr.y), curr.n))
                {
                    continue;
                }

                if (remaining == 0)
                {
                    continue;
                }

                foreach (var (c, (nx, ny)) in input.Neighbours(curr.x, curr.y, true, true, false))
                {
                    if (input[nx][ny] == '#' || visited.ContainsKey((nx, ny)))
                    {
                        continue;
                    }

                    if (c.IsNumeric())
                    {
                        remaining--;
                    }
                    toVisit.Enqueue((nx, ny, curr.n + 1));
                }
            }

            foreach (var c in Enumerable.Range(i + 1, maxN - i).Select(x => (char)('0' + x)))
            {
                var pos = input.FindFirst(c);
                try
                {
                    graph.AddEdges(
                        new Edge<int>(
                            new Vertex(i.ToString()),
                            new Vertex(c.ToString()),
                            visited[(pos.i, pos.j)]));
                }
                catch
                {
                    // Noop
                }
            }
        }


        return graph.ShortestPath(graph.Vertices.First(x => x.Name == "0")).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();

        var maxN = input.SelectMany(x => x).Where(x => x.IsNumeric()).Max() - '0';
        var graph = new Graph<int>();

        for (var i = 0; i < maxN; i++)
        {
            var remaining = maxN - i;
            var start = input.FindFirst((char)('0' + i));
            input[start.i][start.j] = '.';

            var visited = new Dictionary<(int, int), int>();
            var toVisit = new Queue<(int x, int y, int n)>();
            toVisit.Enqueue((start.i, start.j, 0));

            while (toVisit.TryDequeue(out var curr))
            {
                if (!visited.TryAdd((curr.x, curr.y), curr.n))
                {
                    continue;
                }

                if (remaining == 0)
                {
                    continue;
                }

                foreach (var (c, (nx, ny)) in input.Neighbours(curr.x, curr.y, true, true, false))
                {
                    if (input[nx][ny] == '#' || visited.ContainsKey((nx, ny)))
                    {
                        continue;
                    }

                    if (c.IsNumeric())
                    {
                        remaining--;
                    }
                    toVisit.Enqueue((nx, ny, curr.n + 1));
                }
            }

            foreach (var c in Enumerable.Range(i + 1, maxN - i).Select(x => (char)('0' + x)))
            {
                var pos = input.FindFirst(c);
                try
                {
                    graph.AddEdges(
                        new Edge<int>(
                            new Vertex(i.ToString()),
                            new Vertex(c.ToString()),
                            visited[(pos.i, pos.j)]));
                }
                catch
                {
                    // Noop
                }
            }
        }


        return graph.ShortestPath(graph.Vertices.First(x => x.Name == "0"), true).ToString();
    }
}