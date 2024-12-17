using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText().Split("\r\n\r\n");
        var register = input[0].Split("\r\n").Select(x => long.Parse(x.Split(": ")[1])).ToList();
        var program = input[1].Split(": ")[1].Split(',').Select(int.Parse).ToList();

        var result = new List<long>();
        for (var i = 0; i < program.Count; i += 2)
        {
            var instruction = program[i];
            var next = program[i + 1];
            var combo = next <= 3 ? next : register[next - 4];
            switch (instruction)
            {
                case 0:
                    register[0] /= (long)Math.Pow(2, combo);
                    break;
                case 1:
                    register[1] ^= next;
                    break;
                case 2:
                    register[1] = combo & 0b111;
                    break;
                case 3:
                    if (register[0] == 0) break;
                    i = next - 2;
                    break;
                case 4:
                    register[1] ^= register[2];
                    break;
                case 5:
                    result.Add(combo & 0b111);
                    break;
                case 6:
                    register[1] = register[0] / (long)Math.Pow(2, combo);
                    break;
                case 7:
                    register[2] = register[0] / (long)Math.Pow(2, combo);
                    break;
            }
        }

        return string.Join(',', result);
    }

    public string SolveSecond()
    {
        var program = this.InputToText()
            .Split("\r\n\r\n")[1]
            .Split(": ")[1]
            .Split(',')
            .Select(long.Parse)
            .Reverse()
            .ToList();

        var meta = new HashSet<long> { 0 };
        foreach (var num in program)
        {
            var newMeta = new HashSet<long>();
            foreach (var currNum in meta)
            {
                for (var i = 0; i < 8; i++)
                {
                    var newNum = (currNum << 3) + i;
                    if (GetOut(newNum) == num)
                    {
                        newMeta.Add(newNum);
                    }
                }
            }

            meta = newMeta;
        }

        return meta.Min().ToString();
    }

    private static long GetOut(long a)
    {
        var temp = (a % 8) ^ 3;
        return (temp ^ 5 ^ a / (long)Math.Pow(2, temp)) % 8;
    }
}