using System.Text;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day21 : IRiddle
{
    private readonly Dictionary<char, (int, int)> _keypad = new()
    {
        { '7', (0, 0) }, { '8', (0, 1) }, { '9', (0, 2) }, { '4', (1, 0) }, { '5', (1, 1) }, { '6', (1, 2) },
        { '1', (2, 0) }, { '2', (2, 1) }, { '3', (2, 2) }, { '0', (3, 1) }, { 'A', (3, 2) }
    };

    private readonly Dictionary<char, (int, int)> _directions = new()
    {
        { '^', (0, 1) }, { 'A', (0, 2) }, { '<', (1, 0) }, { 'v', (1, 1) }, { '>', (1, 2) },
    };

    private readonly Dictionary<(string, int), long> _cache = [];

    public string SolveFirst()
    {
        _cache.Clear();

        return this.InputToLines()
            .Sum(line => Directions(line, 3, 3) * long.Parse(line[..^1]))
            .ToString();
    }

    public string SolveSecond()
    {
        _cache.Clear();

        return this.InputToLines()
            .Sum(line => Directions(line, 26, 26) * long.Parse(line[..^1]))
            .ToString();
    }

    private long Directions(string str, int depth, int max)
    {
        if (depth == 0)
        {
            return str.Length;
        }

        if (_cache.ContainsKey((str, depth)))
        {
            return _cache[(str, depth)];
        }

        var positions = depth == max ? _keypad : _directions;

        var sum = 0L;

        var (x, y) = positions['A'];
        foreach (var c in str)
        {
            var (nx, ny) = positions[c];
            var s = ShortestPath((x, y), (nx, ny), positions);
            sum += Directions(s, depth - 1, max);
            (x, y) = (nx, ny);
        }

        _cache.Add((str, depth), sum);
        return sum;
    }

    private static string ShortestPath((int x, int y) from, (int x, int y) to, IReadOnlyDictionary<char, (int, int)> positions)
    {
        var sb = new StringBuilder();
        var (dx, dy) = (to.x - from.x, to.y - from.y);

        var h = string.Empty;
        var v = string.Empty;

        while (dy > 0)
        {
            h += '>';
            dy--;
        }

        while (dy < 0)
        {
            h += '<';
            dy++;
        }

        while (dx > 0)
        {
            v += 'v';
            dx--;
        }

        while (dx < 0)
        {
            v += '^';
            dx++;
        }

        if (to.y > from.y && positions.Any(pos => pos.Value == (to.x, from.y)))
        {
            sb.Append(v);
            sb.Append(h);
        }
        else if (positions.Any(pos => pos.Value == (from.x, to.y)))
        {
            sb.Append(h);
            sb.Append(v);
        }
        else if (positions.Any(pos => pos.Value == (to.x, from.y)))
        {
            sb.Append(v);
            sb.Append(h);
        }
        else
        {
            throw new Exception("Should never happen");
        }

        sb.Append('A');

        return sb.ToString();
    }
}