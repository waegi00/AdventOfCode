using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();
        var comparer = Comparer<int>.Create((a, b) => a - b);
        var q1 = new PriorityQueue<int, int>(comparer);
        var q2 = new PriorityQueue<int, int>(comparer);

        foreach (var line in input)
        {
            var splits = line.Split("   ");
            var i1 = int.Parse(splits[0]);
            var i2 = int.Parse(splits[1]);
            q1.Enqueue(i1, i1);
            q2.Enqueue(i2, i2);
        }

        var res = 0;

        while (q1.Count > 0)
        {
            res += Math.Abs(q1.Dequeue() - q2.Dequeue());
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var d1 = new Dictionary<int, int>();
        var d2 = new Dictionary<int, int>();

        foreach (var line in input)
        {
            var splits = line.Split("   ");
            var i1 = int.Parse(splits[0]);
            var i2 = int.Parse(splits[1]);
            if (!d1.TryAdd(i1, 1))
            {
                d1[i1] += 1;
            }
            if (!d2.TryAdd(i2, 1))
            {
                d2[i2] += 1;
            }
        }

        var sum = d1.Keys.Sum(k => k * d1[k] * d2.GetValueOrDefault(k, 0));

        return sum.ToString();
    }
}