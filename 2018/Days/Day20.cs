using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day20 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        var directions = new Dictionary<char, (int, int)>
        {
            { 'N', (0, -1) },
            { 'E', (1, 0) },
            { 'S', (0, 1) },
            { 'W', (-1, 0) }
        };

        var positions = new Stack<(int, int)>();
        int x = 5000, y = 5000;
        var m = new Dictionary<(int, int), HashSet<(int, int)>>();
        int prevX = x, prevY = y;
        var distances = new Dictionary<(int, int), int>();

        for (var i = 1; i < input.Length - 1; i++)
        {
            var c = input[i];

            switch (c)
            {
                case '(':
                    positions.Push((x, y));
                    break;
                case ')':
                    (x, y) = positions.Pop();
                    break;
                case '|':
                    (x, y) = positions.Peek();
                    break;
                default:
                {
                    var (dx, dy) = directions[c];
                    x += dx;
                    y += dy;

                    if (!m.ContainsKey((x, y)))
                    {
                        m[(x, y)] = [];
                    }
                    m[(x, y)].Add((prevX, prevY));

                    if (distances.ContainsKey((x, y)))
                    {
                        distances[(x, y)] = Math.Min(distances[(x, y)], distances[(prevX, prevY)] + 1);
                    }
                    else
                    {
                        distances[(x, y)] = distances.ContainsKey((prevX, prevY)) ? distances[(prevX, prevY)] + 1 : 1;
                    }

                    break;
                }
            }
            prevX = x;
            prevY = y;
        }

        return distances.Values.Max().ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        var directions = new Dictionary<char, (int, int)>
        {
            { 'N', (0, -1) },
            { 'E', (1, 0) },
            { 'S', (0, 1) },
            { 'W', (-1, 0) }
        };

        var positions = new Stack<(int, int)>();
        int x = 5000, y = 5000;
        var m = new Dictionary<(int, int), HashSet<(int, int)>>();
        int prevX = x, prevY = y;
        var distances = new Dictionary<(int, int), int>();

        for (var i = 1; i < input.Length - 1; i++)
        {
            var c = input[i];

            switch (c)
            {
                case '(':
                    positions.Push((x, y));
                    break;
                case ')':
                    (x, y) = positions.Pop();
                    break;
                case '|':
                    (x, y) = positions.Peek();
                    break;
                default:
                {
                    var (dx, dy) = directions[c];
                    x += dx;
                    y += dy;

                    if (!m.ContainsKey((x, y)))
                    {
                        m[(x, y)] = [];
                    }
                    m[(x, y)].Add((prevX, prevY));

                    if (distances.ContainsKey((x, y)))
                    {
                        distances[(x, y)] = Math.Min(distances[(x, y)], distances[(prevX, prevY)] + 1);
                    }
                    else
                    {
                        distances[(x, y)] = distances.ContainsKey((prevX, prevY)) ? distances[(prevX, prevY)] + 1 : 1;
                    }

                    break;
                }
            }
            prevX = x;
            prevY = y;
        }

        return distances.Values.Count(v => v >= 1000).ToString();
    }
}