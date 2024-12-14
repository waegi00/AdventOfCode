using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2024.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" ").Select(y => y.Split(",")).ToList()).ToList();

        const int width = 101;
        const int height = 103;
        const int steps = 100;

        List<(int x, int y, int vx, int vy)> robots = [];
        foreach (var line in input)
        {
            var x = int.Parse(line[0][0][2..]);
            var y = int.Parse(line[0][1]);
            var vx = int.Parse(line[1][0][2..]);
            var vy = int.Parse(line[1][1]);
            robots.Add((x, y, vx, vy));
        }

        robots = robots.Select(r => ((r.x + r.vx * steps).Mod(width), (r.y + r.vy * steps).Mod(height), r.vx, r.vy)).ToList();

        var q1 = robots.Count(r => r is { x: <= (width / 2) - 1, y: <= (height / 2) - 1 });
        var q2 = robots.Count(r => r is { x: >= (width / 2) + 1, y: <= (height / 2) - 1 });
        var q3 = robots.Count(r => r is { x: <= (width / 2) - 1, y: >= (height / 2) + 1 });
        var q4 = robots.Count(r => r is { x: >= (width / 2) + 1, y: >= (height / 2) + 1 });

        return (q1 * q2 * q3 * q4).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(" ").Select(y => y.Split(",")).ToList()).ToList();

        const int width = 101;
        const int height = 103;

        List<(int x, int y, int vx, int vy)> robots = [];
        foreach (var line in input)
        {
            var x = int.Parse(line[0][0][2..]);
            var y = int.Parse(line[0][1]);
            var vx = int.Parse(line[1][0][2..]);
            var vy = int.Parse(line[1][1]);
            robots.Add((x, y, vx, vy));
        }

        var steps = 0;
        while (true)
        {
            robots = robots.Select(r => ((r.x + r.vx).Mod(width), (r.y + r.vy).Mod(height), r.vx, r.vy)).ToList();
            steps++;
            if (robots.Count == new HashSet<(int, int)>(robots.Select(r => (r.x, r.y))).Count)
            {
                break;
            }
        }

        // Draw christmas tree
        // var a = new HashSet<(int, int)>(robots.Select(r => (r.x, r.y)));
        // for (var x = 0; x < width; x++)
        // {
        //     for (var y = 0; y < height; y++)
        //     {
        //         Console.Write(a.Contains((y, x)) ? 'X' : ' ');
        //     }
        //     Console.WriteLine();
        // }

        return steps.ToString();
    }
}