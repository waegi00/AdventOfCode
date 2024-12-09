using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Collections.Generic;

namespace AdventOfCode._2024.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        var ns = new LinkedList<(long, int)>();
        var spaces = new List<long>();

        var i = 0L;
        var isSpace = false;
        foreach (var n in input.Select(c => c - '0'))
        {
            if (isSpace)
            {
                spaces.Add(n);
            }
            else
            {
                ns.AddLast((i++, n));
            }
            isSpace = !isSpace;
        }

        var nums = new List<long>();
        while (ns.Count > 0 && spaces.Count > 0)
        {
            var first = ns.First!;
            nums.AddRange(Enumerable.Repeat(first.Value.Item1, first.Value.Item2));
            ns.RemoveFirst();

            var space = (int)spaces[0];

            while (space > 0 && ns.Count > 0)
            {
                var last = ns.Last!;
                if (last.Value.Item2 > space)
                {
                    last.Value = (last.Value.Item1, last.Value.Item2 - space);
                    nums.AddRange(Enumerable.Repeat(last.Value.Item1, space));
                    space = 0;
                }
                else
                {
                    nums.AddRange(Enumerable.Repeat(last.Value.Item1, last.Value.Item2));
                    space -= last.Value.Item2;
                    ns.RemoveLast();
                }
            }

            spaces.RemoveAt(0);
        }
        nums.AddRange(ns.SelectMany(x => Enumerable.Repeat(x.Item1, x.Item2)));

        return nums.Where(n => n != -1)
            .Select((n, i) => (n, i))
            .Sum(x => x.n * x.i)
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        var ns = new LinkedList<(long index, int num, int pos)>();
        var spaces = new List<(long num, int pos)>();

        var i = 0L;
        var pos = 0;
        var isSpace = false;
        foreach (var n in input.Select(c => c - '0'))
        {
            if (isSpace)
            {
                spaces.Add((n, pos));
            }
            else
            {
                ns.AddLast((i++, n, pos));
            }

            pos += n;
            isSpace = !isSpace;
        }

        var nums = new List<long>();

        for (var k = 0; k < ns.Count; k++)
        {
            nums.AddRange(Enumerable.Repeat(ns.ElementAt(k).index, ns.ElementAt(k).num));
            if (k < spaces.Count) nums.AddRange(Enumerable.Repeat(-1L, (int)spaces.ElementAt(k).num));
        }

        while (ns.Count > 0)
        {
            var item = ns.Last!;
            var spacePos = 0;
            while (spacePos < spaces.Count)
            {
                if (spaces[spacePos].num >= item.Value.num && spaces[spacePos].pos < item.Value.pos)
                {
                    nums = nums.Select(n => n == item.Value.index ? 0 : n).ToList();
                    nums.RemoveRange(spaces[spacePos].pos, item.Value.num);
                    nums.InsertRange(spaces[spacePos].pos, Enumerable.Repeat(item.Value.index, item.Value.num));
                    spaces[spacePos] = (spaces[spacePos].num - item.Value.num, spaces[spacePos].pos + item.Value.num);
                    spacePos = spaces.Count;
                }

                spacePos++;
            }
            ns.RemoveLast();
        }

        return nums.Select(n => n == -1 ? 0 : n)
            .Select((n, i) => (n, i))
            .Sum(x => x.n * x.i)
            .ToString();
    }
}