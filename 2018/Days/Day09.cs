using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(' ');

        return Play(int.Parse(input[0]), int.Parse(input[^2])).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(' ');

        return Play(int.Parse(input[0]), int.Parse(input[^2]) * 100).ToString();
    }

    private static long Play(int playerCount, int lastMarble)
    {
        var scores = new long[playerCount];
        var circle = new LinkedList<long>();
        var current = circle.AddFirst(0);

        for (var marble = 1; marble <= lastMarble; marble++)
        {
            var player = marble % playerCount;

            if (marble % 23 == 0)
            {
                for (var i = 0; i < 7; i++)
                {
                    current = current!.Previous ?? circle.Last;
                }

                scores[player] += marble + current!.Value;

                var toRemove = current;
                current = toRemove.Next ?? circle.First;
                circle.Remove(toRemove);
            }
            else
            {
                current = current!.Next ?? circle.First;
                current = circle.AddAfter(current!, marble);
            }
        }

        return scores.Max();
    }
}