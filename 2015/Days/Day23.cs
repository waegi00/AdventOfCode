using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        return Run(this.InputToLines(), 0, 0).b.ToString();
    }

    public string SolveSecond()
    {
        return Run(this.InputToLines(), 1, 0).b.ToString();
    }

    private static (int a, int b) Run(string[] input, int a, int b)
    {
        for (var i = 0; i < input.Length;)
        {
            var line = input[i];
            switch (line[..3])
            {
                case "hlf":
                    if (line[4] == 'a') { a /= 2; } else { b /= 2; }
                    i++;
                    break;
                case "tpl":
                    if (line[4] == 'a') { a *= 3; } else { b *= 3; }
                    i++;
                    break;
                case "inc":
                    if (line[4] == 'a') { a++; } else { b++; }
                    i++;
                    break;
                case "jmp":
                    i += int.Parse(line[4..]);
                    break;
                case "jie":
                    if (line[4] == 'a' && a % 2 == 0 || line[4] == 'b' && b % 2 == 0) { i += int.Parse(line[7..]); } else { i++; }
                    break;
                case "jio":
                    if (line[4] == 'a' && a == 1 || line[4] == 'b' && b == 1) { i += int.Parse(line[7..]); } else { i++; }
                    break;
            }
        }

        return (a, b);
    }
}