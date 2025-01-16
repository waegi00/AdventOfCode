using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(": ").Select(int.Parse).ToArray())
            .ToArray();

        var l = input.Max(x => x[0]) + 1;
        var fw = new int[l][];
        for (var i = 0; i < l; i++)
        {
            fw[i] = new int[input.FirstOrDefault(x => x[0] == i)?[1] ?? 0];
        }

        var severity = 0;
        for (var i = 0; i < l; i++)
        {
            if (i % ((fw[i].Length - 1) * 2) == 0 && fw[i].Length > 0)
            {
                severity += i * fw[i].Length;
            }
        }

        return severity.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(": ").Select(int.Parse).ToArray())
            .ToArray();

        var l = input.Max(x => x[0]) + 1;
        var fw = new int[l][];
        for (var i = 0; i < l; i++)
        {
            fw[i] = new int[input.FirstOrDefault(x => x[0] == i)?[1] ?? 0];
        }

        var res = 0;

        while (fw.Where((t, i) => t.Length > 0 && (res + i) % ((fw[i].Length - 1) * 2) == 0).Any())
        {
            res++;
        }

        return res.ToString();
    }
}