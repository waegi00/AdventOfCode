using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var disks = this.InputToLines()
            .Select(x => x.Split(' '))
            .Select(x => new[] { x[1][1..], x[3], x[6].Split('=')[1][..^1], x[11][..^1] })
            .Select(x => x.Select(int.Parse).ToList())
            .Select(x => new Disk(x[0], x[1], x[2], x[3]))
            .ToList();

        var time = 0;
        while (true)
        {
            var i = 1;
            foreach (var _ in disks.TakeWhile(disk => (disk.Position + i + time) % disk.Positions == 0))
            {
                if (i == disks.Count)
                {
                    return time.ToString();
                }
                i++;
            }
            time++;
        }
    }

    public string SolveSecond()
    {
        var disks = this.InputToLines()
            .Select(x => x.Split(' '))
            .Select(x => new[] { x[1][1..], x[3], x[6].Split('=')[1][..^1], x[11][..^1] })
            .Select(x => x.Select(int.Parse).ToList())
            .Select(x => new Disk(x[0], x[1], x[2], x[3]))
            .ToList();

        disks.Add(new Disk(disks.Max(d => d.Id) + 1, 11, 0, 0));

        var time = 0;
        while (true)
        {
            var i = 1;
            foreach (var _ in disks.TakeWhile(disk => (disk.Position + i + time) % disk.Positions == 0))
            {
                if (i == disks.Count)
                {
                    return time.ToString();
                }
                i++;
            }
            time++;
        }
    }

    private record Disk(int Id, int Positions, int Time, int Position);
}
