using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        var data = this.InputToText()
            .Select(x => (long)x - '0')
            .ToArray();

        var one = Run(0, 100, data);

        return Enumerable.Range(0, data.Length - 1)
            .Aggregate(string.Empty, (s, _) => s + (one = one.Next!).Value);
    }

    public string SolveSecond()
    {
        var data = this.InputToText()
            .Select(x => (long)x - '0')
            .ToArray();

        var two = Run(1000000 - data.Length, 10000000, data);
        return (two.Next!.Value * two.Next.Next!.Value).ToString();
    }


    private static Cup Run(int extraCups, int rounds, long[] data)
    {
        var cups = data.Select(v => new Cup { Value = v }).ToList();
        cups.AddRange(Enumerable.Range((int)cups.Max(c => c.Value) + 1, extraCups).Select(i => new Cup { Value = i }));

        for (var i = 0; i < cups.Count; ++i)
        {
            cups[i].Next = cups[(i + 1) % cups.Count];
        }

        var map = cups.ToDictionary(c => c.Value, c => c);

        var current = cups[0];
        for (var i = 0; i < rounds; ++i)
        {
            var p = current!.Pick();
            var v = current.Value != 1 ? current.Value - 1 : cups.Count;
            while (p.Value == v || p.Next!.Value == v || p.Next.Next!.Value == v)
            {
                v -= 1;
                if (v == 0) v = cups.Count;
            }
            map[v].Insert(p);
            current = current.Next;
        }
        return map[1];
    }

    private class Cup
    {
        public Cup? Next { get; set; }
        public long Value { get; init; }

        public void Insert(Cup pick)
        {
            var old = Next;
            pick.Next!.Next!.Next = old;
            Next = pick;
        }

        public Cup Pick()
        {
            var pick = Next;
            Next = pick!.Next!.Next!.Next;
            pick.Next.Next.Next = null;
            return pick;
        }
    }
}