using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        return React(this.InputToText()).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        return input.ToHashSet()
            .Select(agent => input
                .Replace(agent.ToString(), "")
                .Replace(char.ToUpper(agent).ToString(), ""))
            .Select(React)
            .Prepend(int.MaxValue)
            .Min()
            .ToString();
    }

    private static int React(string line)
    {
        var buf = new List<char>();

        foreach (var c in line)
        {
            if (buf.Count > 0 && AreOpposite(c, buf[^1]))
            {
                buf.RemoveAt(buf.Count - 1);
            }
            else
            {
                buf.Add(c);
            }
        }

        return buf.Count;

    }

    private static bool AreOpposite(char a, char b)
    {
        return char.ToLower(a) == char.ToLower(b) && (char.IsUpper(a) && char.IsLower(b) || char.IsLower(a) && char.IsUpper(b));
    }
}