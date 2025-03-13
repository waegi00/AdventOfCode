using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var data = this.InputToLines()
            .Select(Convert)
            .ToList();

        var baseList = new List<object>();
        foreach (var expr in data)
        {
            baseList = Add(baseList, expr);
            baseList = Actions(baseList);
        }

        return Magnitude(baseList).ToString();
    }

    public string SolveSecond()
    {
        var data = this.InputToLines()
            .Select(Convert)
            .ToList();

        var max = 0;
        foreach (var a in data)
        {
            foreach (var b in data)
            {
                if (a.SequenceEqual(b)) continue;
                max = Math.Max(max, Magnitude(Actions(Add(a, b))));
            }
        }
        return max.ToString();
    }

    private static List<object> Convert(string ln)
    {
        return ln.Trim()
            .Select(c => 
                char.IsDigit(c) 
                    ? (object)(c - '0') 
                    : c.ToString())
            .ToList();
    }

    private static List<object> Add(List<object> a, List<object> b)
    {
        return a.Count == 0 
            ? b 
            : new List<object> { "[" }
                .Concat(a)
                .Concat(new List<object> { "," })
                .Concat(b)
                .Concat(new List<object> { "]" })
                .ToList();
    }

    private static bool IsDigit(object k)
    {
        return k is int;
    }

    private static List<object> Explode(List<object> a, int n)
    {
        var left = (int)a[n + 1];
        var right = (int)a[n + 3];

        for (var nn = n - 1; nn >= 0; nn--)
        {
            if (!IsDigit(a[nn])) continue;
            a[nn] = (int)a[nn] + left;
            break;
        }

        for (var nn = n + 5; nn < a.Count; nn++)
        {
            if (!IsDigit(a[nn])) continue;
            a[nn] = (int)a[nn] + right;
            break;
        }

        return a.Take(n)
            .Concat(new List<object> { 0 })
            .Concat(a.Skip(n + 5))
            .ToList();
    }

    private static List<object> Split(List<object> a, int n)
    {
        var val = (int)a[n];
        return a.Take(n)
                .Concat(new List<object> { "[", val / 2, ",", (val + 1) / 2, "]" })
                .Concat(a.Skip(n + 1))
                .ToList();
    }

    private static List<object> Actions(List<object> a)
    {
        var changed = true;
        while (changed)
        {
            changed = false;
            var depth = 0;
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i].Equals("]"))
                {
                    depth--;
                }
                else if (a[i].Equals("["))
                {
                    depth++;
                    if (depth != 5) continue;
                    a = Explode(a, i);
                    changed = true;
                    break;
                }
            }
            if (changed) continue;
            for (var i = 0; i < a.Count; i++)
            {
                if (!IsDigit(a[i]) || (int)a[i] < 10) continue;
                a = Split(a, i);
                changed = true;
                break;
            }
        }
        return a;
    }

    private static int Magnitude(List<object> a)
    {
        while (a.Count > 1)
        {
            for (var i = 0; i < a.Count - 2; i++)
            {
                if (!IsDigit(a[i]) || !IsDigit(a[i + 2])) continue;
                a = a.Take(i - 1)
                    .Concat(new List<object> { (int)a[i] * 3 + (int)a[i + 2] * 2 })
                    .Concat(a.Skip(i + 4))
                    .ToList();
                break;
            }
        }
        return (int)a[0];
    }
}