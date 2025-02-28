using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines()
            .Select(x => x.Replace(" ", string.Empty))
            .Sum(x => Evaluate(x.GetEnumerator(), "LR"))
            .ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines()
            .Select(x => x.Replace(" ", string.Empty))
            .Sum(x => Evaluate(x.GetEnumerator(), "+*"))
            .ToString();
    }

    private static readonly Dictionary<char, Func<long, long, long>> Operators = new()
    {
        {'*', (a, b) => a * b},
        {'+', (a, b) => a + b}
    };


    private static long Evaluate(IEnumerator<char> data, string precedence)
    {
        Func<long, long, long>? operate = null;
        var acc = 0L;

        while (data.MoveNext() && data.Current != ')')
        {
            var current = data.Current;

            if (Operators.TryGetValue(current, out var @operator))
            {
                operate = @operator;
                if (precedence[^1] == current || precedence[^1] == 'L')
                {
                    return operate(acc, Evaluate(data, precedence));
                }
                continue;
            }

            var value = current == '(' ? Evaluate(data, precedence) : (long)char.GetNumericValue(current);
            acc = operate?.Invoke(acc, value) ?? value;
        }

        return acc;
    }
}