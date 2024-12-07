using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var sum = 0;

        foreach (var line in input)
        {
            var splits = line.Split(" ").Select(int.Parse).ToList();
            var isAsc = splits[0] < splits[1];

            var i = 0;
            for (; i < splits.Count - 1; i++)
            {
                if (isAsc)
                {
                    if (splits[i] >= splits[i + 1] || splits[i] < splits[i + 1] - 3)
                    {
                        i = int.MaxValue - 1;
                    }
                }
                else
                {
                    if (splits[i] <= splits[i + 1] || splits[i] > splits[i + 1] + 3)
                    {
                        i = int.MaxValue - 1;
                    }
                }
            }

            if (i != int.MaxValue)
            {
                sum++;
            }

        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        return Solve2(this.InputToText(), false);
    }

    private static string Solve2(string input, bool error)
    {
        var sum = 0;

        foreach (var line in input.Split("\r\n"))
        {
            var splits = line.Split(" ").Select(int.Parse).ToList();
            var isAsc = splits[0] < splits[1];
            var i = 0;
            for (; i < splits.Count - 1; i++)
            {
                if (isAsc)
                {
                    if (splits[i] >= splits[i + 1] || splits[i] < splits[i + 1] - 3)
                    {
                        if (!error)
                        {
                            if (Solve2(string.Join(" ", splits.Where((x, index) => index != i).ToList()), true) == "0" &&
                                Solve2(string.Join(" ", splits.Where((x, index) => index != i + 1).ToList()), true) == "0")
                            {
                                i = int.MaxValue - 1;
                            }

                            error = true;
                        }
                        else
                        {
                            i = int.MaxValue - 1;
                        }
                    }
                }
                else
                {
                    if (splits[i] <= splits[i + 1] || splits[i] > splits[i + 1] + 3)
                    {
                        if (!error)
                        {
                            if (Solve2(string.Join(" ", splits.Where((x, index) => index != i).ToList()), true) == "0" &&
                                Solve2(string.Join(" ", splits.Where((x, index) => index != i + 1).ToList()), true) == "0")
                            {
                                i = int.MaxValue - 1;
                            }

                            error = true;
                        }
                        else
                        {
                            i = int.MaxValue - 1;
                        }
                    }
                }
            }

            if (i != int.MaxValue)
            {
                sum++;
            }

            error = false;

        }

        return sum.ToString();
    }
}