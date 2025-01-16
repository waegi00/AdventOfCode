using AdventOfCode.Interfaces;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var lengths = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var list = Enumerable.Range(0, 256).ToArray();
        var curr = 0;
        var skip = 0;

        foreach (var length in lengths)
        {
            for (var i = 0; i < length / 2; i++)
            {
                var iStart = (curr + i) % list.Length;
                var iEnd = (curr + length - 1 - i) % list.Length;
                (list[iStart], list[iEnd]) = (list[iEnd], list[iStart]);
            }
            curr += (length + skip) % list.Length;
            skip++;
        }

        return (list[0] * list[1]).ToString();
    }

    public string SolveSecond()
    {
        return KnotHash(this.InputToText().Select(x => (byte)x).ToList());
    }

    public static string KnotHash(List<byte> lengths)
    {
        lengths.AddRange([17, 31, 73, 47, 23]);

        var list = Enumerable.Range(0, 256)
            .Select(x => (byte)x)
            .ToList();

        var curr = 0;
        var skip = 0;

        for (var rep = 0; rep < 64; rep++)
        {
            foreach (var length in lengths)
            {
                for (var i = 0; i < length / 2; i++)
                {
                    var iStart = (curr + i) % list.Count;
                    var iEnd = (curr + length - 1 - i) % list.Count;
                    (list[iStart], list[iEnd]) = (list[iEnd], list[iStart]);
                }
                curr += (length + skip) % list.Count;
                skip++;
            }
        }

        return '\0'.Join(Enumerable.Range(0, 16)
            .Select(x => list.Skip(x * 16).Take(16))
            .Select(x => x.Aggregate((byte)0, (hash, b) => (byte)(hash ^ b)))
            .Select(x => x.ToString("X2")));
    }
}