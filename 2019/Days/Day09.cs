using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        var computer = new IntCodeComputer(input);

        var result = computer.Run(1).ToList();
        return string.Join(", ", result);
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        var computer = new IntCodeComputer(input);

        var result = computer.Run(2).ToList();
        return string.Join(", ", result);
    }

    private class IntCodeComputer
    {
        private readonly Dictionary<int, int> argcLookup = new()
        {
            {99, 1}, {4, 1}, {3, 1}, {1, 3}, {2, 3}, {5, 2}, {6, 2}, {7, 3}, {8, 3}, {9, 1}
        };

        private readonly long[] memory;
        private int index;
        private int relativeBase;

        public IntCodeComputer(long[] program)
        {
            memory = new long[program.Length + 10000];
            Array.Copy(program, memory, program.Length);
        }

        private static (int[] modes, int opcode) ParseOpcode(int opcode)
        {
            var tempOpcode = opcode.ToString().PadLeft(5, '0');
            return ([tempOpcode[2] - '0', tempOpcode[1] - '0', tempOpcode[0] - '0'], int.Parse(tempOpcode[3..]));
        }

        private long GetValue(int mode, int param)
        {
            return mode switch
            {
                0 => memory[param],
                1 => param,
                2 => memory[param + relativeBase],
                _ => throw new Exception("Invalid mode")
            };
        }

        private int GetWriteIndex(int mode, int param)
        {
            return mode == 2 ? param + relativeBase : param;
        }

        public IEnumerable<long> Run(long input)
        {
            while (true)
            {
                var (modes, opcode) = ParseOpcode((int)memory[index]);
                if (opcode == 99) break;

                var argc = argcLookup[opcode];
                var paramsArray = memory.Skip(index + 1).Take(argc).Select(x => (int)x).ToArray();
                var val1 = argc > 0 ? GetValue(modes[0], paramsArray[0]) : 0;
                var val2 = argc > 1 ? GetValue(modes[1], paramsArray[1]) : 0;
                var outIndex = argc > 2 ? GetWriteIndex(modes[2], paramsArray[2]) : 0;

                switch (opcode)
                {
                    case 1:
                        memory[outIndex] = val1 + val2;
                        break;
                    case 2:
                        memory[outIndex] = val1 * val2;
                        break;
                    case 3:
                        memory[GetWriteIndex(modes[0], paramsArray[0])] = input;
                        break;
                    case 4:
                        yield return val1;
                        break;
                    case 5:
                        if (val1 != 0) { index = (int)val2; continue; }
                        break;
                    case 6:
                        if (val1 == 0) { index = (int)val2; continue; }
                        break;
                    case 7:
                        memory[outIndex] = val1 < val2 ? 1 : 0;
                        break;
                    case 8:
                        memory[outIndex] = val1 == val2 ? 1 : 0;
                        break;
                    case 9:
                        relativeBase += (int)val1;
                        break;
                    default:
                        throw new Exception("Unknown opcode");
                }

                index += argc + 1;
            }
        }
    }
}