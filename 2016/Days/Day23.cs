using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(7);
    }

    public string SolveSecond()
    {
        var first = int.Parse(Solve(7));
        var diff = first - 5040; // 7!, as the input is 7
        return (479001600 + diff).ToString(); // 12! + diff, as the input is 12
    }

    private string Solve(int regA)
    {
        var registers = new[] { regA, 0, 0, 0 };
        var input = this.InputToLines()
            .Select(l => l.Split(' '))
            .ToList();

        var i = 0;
        while (i < input.Count)
        {
            switch (input[i][0])
            {
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
        }

        return registers[0].ToString();
    }
}