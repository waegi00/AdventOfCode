using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        var memory = this.InputToText()
            .Split(',')
            .Select((item, i) => (i: (long)i, x: long.Parse(item)))
            .ToDictionary(item => item.i, item => item.x);

        var sum = 0;
        for (var x = 0; x < 50; x++)
        {
            for (var y = 0; y < 50; y++)
            {
                var output = RunProgram(
                        memory.ToDictionary(),
                        new Queue<long>([x, y]))
                    .ToList();

                if (output[0] == 1)
                {
                    sum++;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var memory = this.InputToText()
            .Split(',')
            .Select((item, i) => (i: (long)i, x: long.Parse(item)))
            .ToDictionary(item => item.i, item => item.x);

        var x = 0;
        var y = 0;
        while (RunProgram(memory.ToDictionary(), new Queue<long>([x + 99, y])).First() != 1)
        {
            y++;
            while (RunProgram(memory.ToDictionary(), new Queue<long>([x, y + 99])).First() != 1)
            {
                x++;
            }
        }

        return (10000 * x + y).ToString();
    }

    private static IEnumerable<long> RunProgram(Dictionary<long, long> memory, Queue<long> input)
    {
        long relBase = 0, pc = 0;
        while (true)
        {
            var opcode = memory[pc] % 100;
            long mode1 = (memory[pc] / 100) % 10, mode2 = (memory[pc] / 1000) % 10, mode3 = (memory[pc] / 10000) % 10;
            var param1 = memory.ContainsKey(pc + 1) ? memory[pc + 1] : 0;
            var param2 = memory.ContainsKey(pc + 2) ? memory[pc + 2] : 0;
            var param3 = memory.ContainsKey(pc + 3) ? memory[pc + 3] : 0;

            switch (opcode)
            {
                case 1:
                    Set(mode3, param3, Get(mode1, param1) + Get(mode2, param2));
                    pc += 4;
                    break;
                case 2:
                    Set(mode3, param3, Get(mode1, param1) * Get(mode2, param2));
                    pc += 4;
                    break;
                case 3:
                    if (input.Count == 0) yield break;
                    Set(mode1, param1, input.Dequeue());
                    pc += 2;
                    break;
                case 4:
                    yield return Get(mode1, param1);
                    pc += 2;
                    break;
                case 5:
                    pc = Get(mode1, param1) != 0 ? Get(mode2, param2) : pc + 3;
                    break;
                case 6:
                    pc = Get(mode1, param1) == 0 ? Get(mode2, param2) : pc + 3;
                    break;
                case 7:
                    Set(mode3, param3, Get(mode1, param1) < Get(mode2, param2) ? 1 : 0);
                    pc += 4;
                    break;
                case 8:
                    Set(mode3, param3, Get(mode1, param1) == Get(mode2, param2) ? 1 : 0);
                    pc += 4;
                    break;
                case 9:
                    relBase += Get(mode1, param1);
                    pc += 2;
                    break;
                case 99:
                    yield break;
                default:
                    throw new Exception("Unknown opcode");
            }

            continue;

            void Set(long mode, long param, long value)
            {
                switch (mode)
                {
                    case 0:
                        memory[param] = value;
                        break;
                    case 2:
                        memory[param + relBase] = value;
                        break;
                }
            }

            long Get(long mode, long param) => mode switch
            {
                0 => memory.GetValueOrDefault(param, 0),
                1 => param,
                2 => memory.ContainsKey(param + relBase) ? memory[param + relBase] : 0,
                _ => throw new Exception("Invalid mode")
            };
        }
    }
}