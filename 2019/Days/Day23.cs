using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        var data = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var processors = Enumerable.Range(0, 50)
            .Select(x => new IntCodeProcessor(data.ToList(), x))
            .ToList();

        var i = 0;
        int? natX = null, natY = null;
        var emptyCount = 0;

        while (true)
        {
            var (address, x, y) = processors[i].RunIntCode();

            if (address != -1 && address != 255)
            {
                processors[(int)address].Io.Enqueue(x);
                processors[(int)address].Io.Enqueue(y);
                emptyCount = 0;
                i = (int)address;
            }
            else if (address == 255)
            {
                return y.ToString();
            }
            else
            {
                if (IsNetworkIdle(processors))
                {
                    if (emptyCount == 10000)
                    {
                        processors[0].Io.Enqueue(natX!.Value);
                        processors[0].Io.Enqueue(natY!.Value);
                        emptyCount = 0;
                    }
                    else
                    {
                        emptyCount++;
                    }
                }
                i = (i + 1) % 50;
            }
        }
    }

    public string SolveSecond()
    {
        var data = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var processors = Enumerable.Range(0, 50)
            .Select(x => new IntCodeProcessor(data.ToList(), x))
            .ToList();

        var i = 0;
        int? natX = null, natY = null, prevNatY = null;
        var emptyCount = 0;

        while (true)
        {
            var (address, x, y) = processors[i].RunIntCode();

            if (address != -1 && address != 255)
            {
                processors[(int)address].Io.Enqueue(x);
                processors[(int)address].Io.Enqueue(y);
                emptyCount = 0;
                i = (int)address;
            }
            else if (address == 255)
            {
                natX = (int)x;
                natY = (int)y;
                emptyCount = 0;
            }
            else
            {
                if (IsNetworkIdle(processors))
                {
                    if (emptyCount == 10000)
                    {
                        if (prevNatY.HasValue && natY == prevNatY)
                        {
                            return natY.Value.ToString();
                        }

                        prevNatY = natY;
                        processors[0].Io.Enqueue(natX!.Value);
                        processors[0].Io.Enqueue(natY!.Value);
                        emptyCount = 0;
                    }
                    else
                    {
                        emptyCount++;
                    }
                }
                i = (i + 1) % 50;
            }
        }
    }

    private static bool IsNetworkIdle(List<IntCodeProcessor> processors)
    {
        return processors.All(x => x.Io.Count == 0);
    }

    private class IntCodeProcessor
    {
        private readonly Dictionary<long, long> program;
        public Queue<long> Io { get; }
        private long pos;
        private long relativeBase;

        public IntCodeProcessor(List<long> code, int input)
        {
            program = new Dictionary<long, long>();
            Io = new Queue<long>();
            Io.Enqueue(input);
            relativeBase = 0;
            pos = 0;

            for (var i = 0; i < code.Count; i++)
            {
                program[i] = code[i];
            }
        }

        private long GetInput()
        {
            return Io.Count > 0 ? Io.Dequeue() : -1;
        }

        private (long addr, long x, long y) ProcessOutput()
        {
            if (Io.Count != 3) return (-1, -1, -1);
            var address = Io.Dequeue();
            var x = Io.Dequeue();
            var y = Io.Dequeue();
            return (address, x, y);
        }

        public (long addr, long x, long y) RunIntCode()
        {
            var paramsMap = new Dictionary<int, int>
            {
                { 1, 3 }, { 2, 3 }, { 3, 1 }, { 4, 1 }, { 5, 2 }, { 6, 2 }, { 7, 3 }, { 8, 3 }, { 9, 1 }, { 99, 0 }
            };

            while (true)
            {
                var instr = program[pos];
                var op = instr % 100;
                var mode1 = (instr / 100) % 10;
                var mode2 = (instr / 1000) % 10;
                var mode3 = (instr / 10000) % 10;

                var reg1 = program.GetValueOrDefault(pos + 1, 0);
                var reg2 = program.GetValueOrDefault(pos + 2, 0);
                var reg3 = program.GetValueOrDefault(pos + 3, 0);

                var v1 = reg1;
                if (op != 3)
                {
                    v1 = mode1 switch
                    {
                        0 => program.GetValueOrDefault(reg1, 0),
                        2 => program.GetValueOrDefault(reg1 + relativeBase, 0),
                        _ => v1
                    };
                }
                else
                {
                    if (mode1 == 2)
                        v1 += relativeBase;
                }

                var v2 = mode2 switch
                {
                    0 => program.GetValueOrDefault(reg2, 0),
                    2 => program.GetValueOrDefault(reg2 + relativeBase, 0),
                    _ => reg2
                };

                var v3 = reg3;
                if (mode3 == 2)
                {
                    v3 += relativeBase;
                }

                switch (op)
                {
                    case 1:
                        program[v3] = v1 + v2;
                        break;
                    case 2:
                        program[v3] = v1 * v2;
                        break;
                    case 3:
                        {
                            program[v1] = GetInput();
                            if (program[v1] == -1)
                            {
                                pos += 2;
                                return (-1, -1, -1);
                            }

                            break;
                        }
                    case 4:
                        {
                            Io.Enqueue(v1);
                            var (address, x, y) = ProcessOutput();
                            if (address != -1)
                            {
                                pos += 2;
                                return (address, x, y);
                            }

                            break;
                        }
                    case 5:
                        {
                            if (v1 > 0)
                            {
                                pos = v2;
                            }
                            else
                            {
                                pos += 3;
                            }
                            continue;
                        }
                    case 6:
                        {
                            if (v1 == 0)
                            {
                                pos = v2;
                            }
                            else
                            {
                                pos += 3;
                            }
                            continue;
                        }
                    case 7:
                        program[v3] = (v1 < v2) ? 1 : 0;
                        break;
                    case 8:
                        program[v3] = (v1 == v2) ? 1 : 0;
                        break;
                    case 9:
                        relativeBase += v1;
                        break;
                    case 99:
                        return ProcessOutput();
                    default:
                        throw new Exception("Unknown opcode encountered");
                }

                pos += paramsMap[(int)op] + 1;
            }
        }
    }
}