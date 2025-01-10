using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var passcode = this.InputToText();

        var visited = new HashSet<(int, int, string)>();
        var queue = new Queue<(int x, int y, string code)>();
        queue.Enqueue((0, 0, passcode));

        while (queue.TryDequeue(out var pos))
        {
            visited.Add(pos);

            if (pos is { x: 3, y: 3 })
            {
                return pos.code[passcode.Length..];
            }

            foreach (var (dx, dy, dir) in Directions(pos.code))
            {
                var nx = pos.x + dx;
                var ny = pos.y + dy;

                if (nx is >= 0 and <= 3 && ny is >= 0 and <= 3 && !visited.Contains((nx, ny, pos.code + dir)))
                {
                    queue.Enqueue((nx, ny, pos.code + dir));
                }
            }
        }

        return "";
    }

    public string SolveSecond()
    {
        var passcode = this.InputToText();

        var visited = new HashSet<(int, int, string)>();
        var queue = new Queue<(int x, int y, string code)>();
        queue.Enqueue((0, 0, passcode));

        var max = 0;
        while (queue.TryDequeue(out var pos))
        {
            visited.Add(pos);

            if (pos is { x: 3, y: 3 })
            {
                max = Math.Max(pos.code.Length - passcode.Length, max);
                continue;
            }

            foreach (var (dx, dy, dir) in Directions(pos.code))
            {
                var nx = pos.x + dx;
                var ny = pos.y + dy;

                if (nx is >= 0 and <= 3 && ny is >= 0 and <= 3 && !visited.Contains((nx, ny, pos.code + dir)))
                {
                    queue.Enqueue((nx, ny, pos.code + dir));
                }
            }
        }

        return max.ToString();
    }

    private static List<(int dx, int dy, char dir)> Directions(string code)
    {
        (int pos, int dx, int dy)[] directions = [
            (0, -1, 0),
            (1, 1, 0),
            (2, 0, -1),
            (3,0, 1)];

        var hash = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(code)));

        return directions
            .Where(d => (char)hash[d.pos] is >= 'B' and <= 'F')
            .Select(d => (d.dx, d.dy, Dir(d.pos)))
            .ToList();
    }

    private static char Dir(int pos)
    {
        return pos switch
        {
            0 => 'U',
            1 => 'D',
            2 => 'L',
            3 => 'R',
            _ => throw new ArgumentOutOfRangeException(nameof(pos))
        };
    }
}
