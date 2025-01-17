using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var infected = new HashSet<(int, int)>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input.Length; j++)
            {
                if (input[i][j] == '#')
                {
                    infected.Add((i, j));
                }
            }
        }

        var half = input.Length / 2;
        var (x, y) = (half, half);
        var (dx, dy) = (-1, 0);

        var res = 0;
        for (var i = 0; i < 10000; i++)
        {

            if (infected.Contains((x, y)))
            {
                (dx, dy) = (dy, -dx);
                infected.Remove((x, y));
            }
            else
            {
                (dx, dy) = (-dy, dx);
                infected.Add((x, y));
                res++;
            }

            (x, y) = (x + dx, y + dy);
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var infected = new Dictionary<(int, int), State>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input.Length; j++)
            {
                infected.Add((i, j), input[i][j] == '#' ? State.Infected : State.Clean);
            }
        }

        var half = input.Length / 2;
        var (x, y) = (half, half);
        var (dx, dy) = (-1, 0);

        var res = 0;
        for (var i = 0; i < 10000000; i++)
        {
            switch (infected.GetValueOrDefault((x, y), State.Clean))
            {
                case State.Clean:
                    (dx, dy) = (-dy, dx);
                    break;
                case State.Weakened:
                    break;
                case State.Infected:
                    (dx, dy) = (dy, -dx);
                    break;
                case State.Flagged:
                    (dx, dy) = (-dx, -dy);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!infected.TryAdd((x, y), Next(State.Clean)))
            {
                infected[(x, y)] = Next(infected[(x, y)]);
                if (infected[(x, y)] == State.Infected)
                {
                    res++;
                }
            }

            (x, y) = (x + dx, y + dy);
        }

        return res.ToString();
    }

    private enum State
    {
        Clean,
        Weakened,
        Infected,
        Flagged
    }

    private static State Next(State curr)
    {
        return curr switch
        {
            State.Clean => State.Weakened,
            State.Weakened => State.Infected,
            State.Infected => State.Flagged,
            State.Flagged => State.Clean,
            _ => throw new ArgumentOutOfRangeException(nameof(curr), curr, null)
        };
    }
}