using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .Select(x => (A: x[1][0], B: x[7][0]))
            .ToList();

        var steps = input
            .Select(x => new[] { x.A, x.B })
            .SelectMany(x => x)
            .Distinct()
            .Order()
            .ToList();

        var result = "";

        while (steps.Count > 0)
        {
            var next = steps.First(x => !input.Select(y => y.B).Contains(x));
            result += next;
            steps.Remove(next);
            input = input.Where(x => x.A != next).ToList();
        }

        return result;
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .Select(x => (A: x[1][0], B: x[7][0]))
            .ToList();

        var steps = input
            .Select(x => new[] { x.A, x.B })
            .SelectMany(x => x)
            .Distinct()
            .Order()
            .ToList();

        var t = 0;
        var workers = new List<(int t, char name)>();

        while (steps.Count > 0 || workers.Count > 0)
        {
            for (var i = workers.Count; i < 5; i++)
            {
                var next = steps.FirstOrDefault(x => !input.Select(y => y.B).Contains(x));
                if (next == '\0') break;
                workers.Add((next - 'A' + 61, next));
                steps.Remove(next);
            }

            workers = workers.Select(w => (w.t - 1, w.name)).ToList();

            foreach (var worker in workers.Where(x => x.t == 0).ToList())
            {
                workers.Remove(worker);
                input = input.Where(x => x.A != worker.name).ToList();
            }

            t++;
        }

        return t.ToString();
    }
}