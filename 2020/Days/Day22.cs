using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        var players = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n")
                .Skip(1)
                .Select(int.Parse)
                .ToArray())
            .ToArray();
        
        var X = new List<int>(players[0]);
        var Y = new List<int>(players[1]);

        while (X.Count > 0 && Y.Count > 0)
        {
            var x = X[0];
            X.RemoveAt(0);

            var y = Y[0];
            Y.RemoveAt(0);

            if (x > y)
            {
                X.Add(x);
                X.Add(y);
            }
            else
            {
                Y.Add(y);
                Y.Add(x);
            }
        }

        return (X.Count > 0 ? X : Y)
            .Select((e, i) => e * ((X.Count > 0 ? X.Count : Y.Count) - i))
            .Sum()
            .ToString();
    }

    public string SolveSecond()
    {
        var players = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n")
                .Skip(1)
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        _ = TryGame([.. players[0]], [.. players[1]], out var winner);
        return winner
            .Select((e, i) => e * (winner.Count - i))
            .Sum()
            .ToString();
    }

    private static bool TryGame(List<int> X, List<int> Y, out List<int> result)
    {
        var seen = new HashSet<string>();
        var flag = false;
        while (X.Count > 0 && Y.Count > 0)
        {
            var state = string.Join(",", X) + "|" + string.Join(",", Y);
            if (!seen.Add(state))
            {
                flag = true;
                break;
            }

            var x = X[0];
            X.RemoveAt(0);
            var y = Y[0];
            Y.RemoveAt(0);

            if (x <= X.Count && y <= Y.Count)
            {
                if (TryGame(X.Take(x).ToList(), Y.Take(y).ToList(), out result))
                {
                    X.Add(x);
                    X.Add(y);
                }
                else
                {
                    Y.Add(y);
                    Y.Add(x);
                }
            }
            else
            {
                if (x > y)
                {
                    X.Add(x);
                    X.Add(y);
                    flag = true;
                }
                else
                {
                    Y.Add(y);
                    Y.Add(x);
                    flag = false;
                }
            }
        }

        result = X.Count > 0 ? X : Y;
        return flag;
    }
}