using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var registers = new int[4];
        var input = this.InputToLines()
            .Select(l => l.Split(' '))
            .ToList();

        var i = 0;
        while (i < input.Count)
        {
            switch (input[i][0])
            {
                case "cpy":
                    registers[input[i][2][0] - 'a'] = int.TryParse(input[i][1], out var a) ? a : registers[input[i][1][0] - 'a'];
                    break;
                case "inc":
                    registers[input[i][1][0] - 'a']++;
                    break;
                case "dec":
                    registers[input[i][1][0] - 'a']--;
                    break;
                case "jnz":
                    if ((int.TryParse(input[i][1], out var b) ? b : registers[input[i][1][0] - 'a']) != 0)
                    {
                        i += int.Parse(input[i][2]) - 1;
                    }
                    break;
            }

            i++;
        }

        return registers[0].ToString();
    }

    public string SolveSecond()
    {
        var registers = new int[] { 0, 0, 1, 0 };
        var input = this.InputToLines()
            .Select(l => l.Split(' '))
            .ToList();

        var i = 0;
        while (i < input.Count)
        {
            switch (input[i][0])
            {
                case "cpy":
                    registers[input[i][2][0] - 'a'] = int.TryParse(input[i][1], out var a) ? a : registers[input[i][1][0] - 'a'];
                    break;
                case "inc":
                    registers[input[i][1][0] - 'a']++;
                    break;
                case "dec":
                    registers[input[i][1][0] - 'a']--;
                    break;
                case "jnz":
                    if ((int.TryParse(input[i][1], out var b) ? b : registers[input[i][1][0] - 'a']) != 0)
                    {
                        i += int.Parse(input[i][2]) - 1;
                    }
                    break;
            }

            i++;
        }

        return registers[0].ToString();
    }
}
