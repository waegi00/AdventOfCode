using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToArray();

        var registers = new long[8];
        var res = 0;

        for (var i = 0L; i >= 0 && i < input.Length; i++)
        {
            var x = input[(int)i][1][0] - 'a';
            var valX = long.TryParse(input[(int)i][1], out var numX) ? numX : registers[x];
            var y = long.TryParse(input[(int)i][2], out var numY)
                ? numY
                : registers[input[(int)i][2][0] - 'a'];

            switch (input[(int)i][0])
            {
                case "set":
                    registers[x] = y;
                    break;
                case "sub":
                    registers[x] -= y;
                    break;
                case "mul":
                    res++;
                    registers[x] *= y;
                    break;
                case "jnz":
                    if (valX != 0)
                    {
                        i += y - 1;
                    }
                    break;
            }
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        const int x = 67 * 100 + 100000;
        var h = 0;
        for (var n = x; n <= x + 17000; n += 17)
        {
            for (var i = 2; i < n; i++)
            {
                if (n % i != 0) continue;
                h += 1;
                break;
            }
        }

        return h.ToString();
    }
}