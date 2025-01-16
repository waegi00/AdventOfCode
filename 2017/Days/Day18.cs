using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToList();

        var registers = new long[26];
        var snd = -1L;

        for (var i = 0L; i < input.Count; i++)
        {
            var x = input[(int)i][1][0] - 'a';
            var valX = long.TryParse(input[(int)i][1], out var numX) ? numX : registers[x];
            var y = input[(int)i].Length == 3 ?
                long.TryParse(input[(int)i][2], out var numY)
                    ? numY
                    : registers[input[(int)i][2][0] - 'a']
                : -1;

            switch (input[(int)i][0])
            {
                case "snd":
                    snd = valX;
                    break;
                case "set":
                    registers[x] = y;
                    break;
                case "add":
                    registers[x] += y;
                    break;
                case "mul":
                    registers[x] *= y;
                    break;
                case "mod":
                    registers[x] %= y;
                    break;
                case "rcv":
                    if (registers[x] != 0)
                    {
                        return snd.ToString();
                    }
                    break;
                case "jgz":
                    if (valX > 0)
                    {
                        i += y - 1;
                    }
                    break;
            }
        }

        return "";
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x => x.Split(' '))
            .ToList();

        var state = new[] { true, true };
        var done = new[] { false, false };
        var lines = new[] { 0L, 0L };
        var snd = new Queue<long>[] { [], [] };
        var registers = new long[2][];
        registers[0] = new long[26];
        registers[1] = new long[26];
        registers[1]['p' - 'a'] = 1;

        var total = 0;

        while (done.Any(x => !x))
        {
            int curr;
            if (state[0] && !done[0])
            {
                curr = 0;
            }
            else if (state[1] && !done[1])
            {
                curr = 1;
            }
            else
            {
                if (snd[0].Count > 0 && !done[0])
                {
                    curr = 0;
                }
                else if (snd[1].Count > 0 && !done[1])
                {
                    curr = 1;
                }
                else
                {
                    done = [true, true];
                    continue;
                }
            }

            var register = registers[curr];

            var i = lines[curr];
            for (; i < input.Count && state[curr]; i++)
            {
                var x = input[(int)i][1][0] - 'a';
                var valX = long.TryParse(input[(int)i][1], out var numX) ? numX : register[x];
                var y = input[(int)i].Length == 3 ?
                    long.TryParse(input[(int)i][2], out var numY)
                        ? numY
                        : register[input[(int)i][2][0] - 'a']
                    : -1;

                switch (input[(int)i][0])
                {
                    case "snd":
                        if (curr == 1)
                        {
                            total++;
                        }
                        snd[(curr + 1) % 2].Enqueue(valX);
                        state[(curr + 1) % 2] = true;
                        break;
                    case "set":
                        register[x] = y;
                        break;
                    case "add":
                        register[x] += y;
                        break;
                    case "mul":
                        register[x] *= y;
                        break;
                    case "mod":
                        register[x] %= y;
                        break;
                    case "rcv":
                        if (snd[curr].Count > 0)
                        {
                            var rec = snd[curr].Dequeue();
                            register[x] = rec;
                        }
                        else
                        {
                            state[curr] = false;
                            lines[curr] = i;
                        }
                        break;
                    case "jgz":
                        if (valX > 0)
                        {
                            i += y - 1;
                        }
                        break;
                }
            }

            if (i < 0 || i >= input.Count)
            {
                done[curr] = true;
            }
        }

        return total.ToString();
    }
}