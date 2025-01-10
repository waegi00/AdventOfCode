using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var i = 0;

        while (!Solve(i))
        {
            i++;
        }

        return i.ToString();
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }

    private bool Solve(int regA)
    {
        var nextIs0 = true;

        var registers = new[] { regA, 0, 0, 0 };
        var input = this.InputToLines()
            .Select(l => l.Split(' '))
            .ToList();

        var i = 0;
        var depth = 0;
        while (i < input.Count && depth < 1_000_000)
        {
            switch (input[i][0])
            {
                case "out":
                    var next = int.TryParse(input[i][1], out var n) ? n : registers[input[i][1][0] - 'a'];
                    if (nextIs0)
                    {
                        if (next != 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (next != 1)
                        {
                            return false;
                        }
                    }

                    nextIs0 = !nextIs0;
                    break;
                case "tgl":
                    try
                    {
                        var pos = int.TryParse(input[i][1], out var b) ? b : registers[input[i][1][0] - 'a'];
                        if (pos + i >= 0 && pos + i < input.Count)
                        {
                            input[pos + i][0] = input[pos + i][0] switch
                            {
                                "inc" => "dec",
                                "dec" => "inc",
                                "tgl" => "inc",
                                "cpy" => "jnz",
                                "jnz" => "cpy",
                                _ => throw new ArgumentOutOfRangeException()
                            };
                        }
                    }
                    catch
                    {
                        // Can happen because of "tgl"
                    }

                    break;
                case "cpy":
                    try
                    {
                        registers[input[i][2][0] - 'a'] =
                            int.TryParse(input[i][1], out var a) ? a : registers[input[i][1][0] - 'a'];
                    }
                    catch
                    {
                        // Can happen because of "tgl"
                    }

                    break;
                case "inc":
                    registers[input[i][1][0] - 'a']++;
                    break;
                case "dec":
                    registers[input[i][1][0] - 'a']--;
                    break;
                case "jnz":
                    try
                    {
                        if ((int.TryParse(input[i][1], out var b) ? b : registers[input[i][1][0] - 'a']) != 0)
                        {
                            i += (int.TryParse(input[i][2], out var c) ? c : registers[input[i][2][0] - 'a']) - 1;
                        }
                    }
                    catch
                    {
                        // Can happen because of "tgl"
                    }

                    break;
            }

            i++;
            depth++;
        }

        return true;
    }
}