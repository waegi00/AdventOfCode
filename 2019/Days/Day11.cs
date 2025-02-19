using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var visited = new HashSet<(int, int)>();
        var white = new HashSet<(int, int)>();

        var direction = 0;
        var pos = (x: 0, y: 0);

        var computer = new IntCodeComputer(input);
        computer.AddInput(white.Contains((pos.x, pos.y)) ? 1 : 0);

        while (!computer.Finished)
        {
            computer.Compute();
            if (computer.OutputArray.Count < 2) break;

            var color = (int)computer.OutputArray[^2];
            var turn = (int)computer.OutputArray[^1];

            visited.Add((pos.x, pos.y));
            if (color == 1)
            {
                white.Add((pos.x, pos.y));
            }
            else
            {
                white.Remove((pos.x, pos.y));
            }

            direction = direction switch
            {
                0 => turn == 0 ? 3 : 1,
                1 => turn == 0 ? 0 : 2,
                2 => turn == 0 ? 1 : 3,
                3 => turn == 0 ? 2 : 0,
                _ => throw new ArgumentOutOfRangeException()
            };

            switch (direction)
            {
                case 0:
                    pos.y--;
                    break;
                case 1:
                    pos.x++;
                    break;
                case 2:
                    pos.y++;
                    break;
                case 3:
                    pos.x--;
                    break;
            }

            computer.AddInput(white.Contains((pos.x, pos.y)) ? 1 : 0);
        }

        return visited.Count.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var visited = new HashSet<(int, int)>();
        var white = new HashSet<(int, int)> { (0, 0) };

        var direction = 0;
        var pos = (x: 0, y: 0);

        var computer = new IntCodeComputer(input);
        computer.AddInput(white.Contains((pos.x, pos.y)) ? 1 : 0);

        while (!computer.Finished)
        {
            computer.Compute();
            if (computer.OutputArray.Count < 2) break;

            var color = (int)computer.OutputArray[^2];
            var turn = (int)computer.OutputArray[^1];

            visited.Add((pos.x, pos.y));
            if (color == 1)
            {
                white.Add((pos.x, pos.y));
            }
            else
            {
                white.Remove((pos.x, pos.y));
            }

            direction = direction switch
            {
                0 => turn == 0 ? 3 : 1,
                1 => turn == 0 ? 0 : 2,
                2 => turn == 0 ? 1 : 3,
                3 => turn == 0 ? 2 : 0,
                _ => throw new ArgumentOutOfRangeException()
            };

            switch (direction)
            {
                case 0:
                    pos.y--;
                    break;
                case 1:
                    pos.x++;
                    break;
                case 2:
                    pos.y++;
                    break;
                case 3:
                    pos.x--;
                    break;
            }

            computer.AddInput(white.Contains((pos.x, pos.y)) ? 1 : 0);
        }

        for (var j = white.Min(x => x.Item2); j <= white.Max(x => x.Item2); j++)
        {
            for (var i = white.Min(x => x.Item1); i <= white.Max(x => x.Item1); i++)
            {
                Console.Write(white.Contains((i, j)) ? "X " : "  ");
            }
            Console.WriteLine();
        }

        return "";
    }

    private class IntCodeComputer
    {
        private long relativeBase;
        private readonly List<long> memory;
        private int index;
        private readonly Queue<long> inputQueue = new();
        public List<long> OutputArray { get; private set; } = [];
        public bool Finished { get; private set; } = false;

        public IntCodeComputer(List<long> programCode, int emptySpace = 1000)
        {
            memory = [.. programCode];
            for (var i = 0; i < emptySpace; i++)
            {
                memory.Add(0);
            }
        }

        public void AddInput(long input)
        {
            inputQueue.Enqueue(input);
        }

        public void Compute()
        {
            while (true)
            {
                var instruction = memory[index].ToString("D5");
                var opCode = int.Parse(instruction.Substring(3));
                int[] modes = { instruction[2] - '0', instruction[1] - '0', instruction[0] - '0' };

                switch (opCode)
                {
                    case 1:
                        SetParam(3, GetParam(1) + GetParam(2));
                        index += 4;
                        break;
                    case 2:
                        SetParam(3, GetParam(1) * GetParam(2));
                        index += 4;
                        break;
                    case 3:
                        if (inputQueue.Count == 0) return;
                        SetParam(1, inputQueue.Dequeue());
                        index += 2;
                        break;
                    case 4:
                        OutputArray.Add(GetParam(1));
                        index += 2;
                        break;
                    case 5:
                        index = GetParam(1) != 0 ? (int)GetParam(2) : index + 3;
                        break;
                    case 6:
                        index = GetParam(1) == 0 ? (int)GetParam(2) : index + 3;
                        break;
                    case 7:
                        SetParam(3, GetParam(1) < GetParam(2) ? 1 : 0);
                        index += 4;
                        break;
                    case 8:
                        SetParam(3, GetParam(1) == GetParam(2) ? 1 : 0);
                        index += 4;
                        break;
                    case 9:
                        relativeBase += GetParam(1);
                        index += 2;
                        break;
                    case 99:
                        Finished = true;
                        return;
                    default:
                        throw new Exception("Unknown opcode encountered: " + opCode);
                }

                continue;

                void SetParam(int offset, long value)
                {
                    var mode = modes[offset - 1];
                    var address = memory[index + offset];
                    if (mode == 2) address += relativeBase;
                    memory[(int)address] = value;
                }

                long GetParam(int offset)
                {
                    var mode = modes[offset - 1];
                    var value = memory[index + offset];
                    return mode switch
                    {
                        0 => memory[(int)value],
                        1 => value,
                        _ => memory[(int)(value + relativeBase)]
                    };
                }
            }
        }
    }

}