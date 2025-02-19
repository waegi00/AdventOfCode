using AdventOfCode.Interfaces;
using System.Drawing;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day13 : IRiddle
{
    private IntCodeVM? vm;
    private readonly Dictionary<Point, long> screen = new();
    private long score;
    private Point paddleCoords;
    private Point ballCoords;


    public string SolveFirst()
    {
        vm = new IntCodeVM(this.InputToText());
        vm.RunProgram();
        screen.Clear();

        ProcessOutputs();

        var ans = screen.Values.Count(v => v == 2);

        return ans.ToString();
    }

    public string SolveSecond()
    {
        vm!.Reset();

        vm.WriteMemory(0, 2);

        while (vm.RunProgram() == HaltType.HALT_WAITING)
        {
            ProcessOutputs();

            if (ballCoords.X < paddleCoords.X)
            {
                vm.WriteInput(-1);
            }
            else if (ballCoords.X > paddleCoords.X)
            {
                vm.WriteInput(1);
            }
            else
            {
                vm.WriteInput(0);
            }
        }

        ProcessOutputs();

        return score.ToString();
    }

    private void ProcessOutputs()
    {
        while (vm!.ReadOutputs().Count > 0)
        {
            var x = vm.PopOutput();
            var y = vm.PopOutput();
            var tile = vm.PopOutput();

            if (x == -1 && y == 0)
            {
                score = tile;
            }

            var p = new Point((int)x, (int)y);

            screen[p] = tile;

            switch (tile)
            {
                case 3:
                    paddleCoords = p;
                    break;
                case 4:
                    ballCoords = p;
                    break;
            }
        }
    }

    private class IntCodeInstruction(IntCodeVM vmInst, long[] parameters, IntCodeMode[] modes)
    {
        public IntCodeOp OpCode;
        public long[] Parameters = parameters;
        public IntCodeMode[] Modes = modes;
        public int Length;

        public long GetParam(int paramNum)
        {
            return Modes[paramNum - 1] switch
            {
                IntCodeMode.POSITION => vmInst.ReadMemory(Parameters[paramNum - 1]),
                IntCodeMode.IMMEDIATE => Parameters[paramNum - 1],
                IntCodeMode.RELATIVE => vmInst.ReadMemory(vmInst.RelativeBase + Parameters[paramNum - 1]),
                _ => throw new FormatException()
            };
        }

        public long GetWriteAddress(int paramNum)
        {
            return Modes[paramNum - 1] switch
            {
                IntCodeMode.POSITION => Parameters[paramNum - 1],
                IntCodeMode.RELATIVE => vmInst.RelativeBase + Parameters[paramNum - 1],
                IntCodeMode.IMMEDIATE => throw new ArgumentException(),
                _ => throw new FormatException()
            };
        }

    }

    private enum IntCodeOp
    {
        ADD = 1,
        MULTIPLY = 2,
        INPUT = 3,
        OUTPUT = 4,
        JUMP_TRUE = 5,
        JUMP_FALSE = 6,
        LT = 7,
        EQUALS = 8,
        ADJUST_REL_BASE = 9,
        HALT = 99
    }

    private enum IntCodeMode
    {
        POSITION = 0,
        IMMEDIATE = 1,
        RELATIVE = 2
    }

    private enum HaltType
    {
        HALT_TERMINATE,
        HALT_WAITING
    }

    private class IntCodeVM
    {
        private readonly long[] program;
        private long[]? memory;
        private readonly Queue<long> Inputs = new();
        private readonly Queue<long> Outputs = new();
        public long RelativeBase;
        private long IP;

        public IntCodeVM(string prog)
        {
            program = prog.Split(',').Select(long.Parse).ToArray();
            memory = new long[program.Length];
            program.CopyTo(memory, 0);
            IP = 0;
        }

        public void Reset()
        {
            if (memory!.Length == program.Length)
            {
                Array.Clear(memory, 0, memory.Length);
            }
            else
            {
                memory = new long[program.Length];
            }

            program.CopyTo(memory, 0);
            IP = 0;
            Inputs.Clear();
            Outputs.Clear();
            RelativeBase = 0;
        }

        public void WriteInput(long input)
        {
            Inputs.Enqueue(input);
        }

        public Queue<long> ReadOutputs()
        {
            return Outputs;
        }

        public long PopOutput()
        {
            return Outputs.Dequeue();
        }

        private IntCodeInstruction ParseOpCode()
        {
            var instr = new IntCodeInstruction(this, [], []);
            var tempOpCode = memory![IP];
            instr.OpCode = (IntCodeOp)(tempOpCode % 100);

            var paramCount = instr.OpCode switch
            {
                IntCodeOp.ADD => 3,
                IntCodeOp.MULTIPLY => 3,
                IntCodeOp.INPUT => 1,
                IntCodeOp.OUTPUT => 1,
                IntCodeOp.JUMP_TRUE => 2,
                IntCodeOp.JUMP_FALSE => 2,
                IntCodeOp.LT => 3,
                IntCodeOp.EQUALS => 3,
                IntCodeOp.ADJUST_REL_BASE => 1,
                _ => 0
            };

            instr.Length = paramCount + 1;

            instr.Parameters = new long[paramCount];
            instr.Modes = new IntCodeMode[paramCount];

            for (var x = 0; x < paramCount; x++)
            {
                instr.Parameters[x] = memory[IP + (x + 1)];
            }

            var accessMask = tempOpCode / 100;

            for (var x = 0; x < paramCount; x++)
            {
                instr.Modes[x] = (IntCodeMode)(accessMask % 10);
                accessMask /= 10;
            }

            return instr;
        }

        public HaltType RunProgram()
        {
            while (true)
            {
                var instr = ParseOpCode();
                var IPModified = false;
                switch (instr.OpCode)
                {
                    case IntCodeOp.HALT:
                        return HaltType.HALT_TERMINATE;
                    case IntCodeOp.ADD:
                        WriteMemory(instr.GetWriteAddress(3), instr.GetParam(1) + instr.GetParam(2));
                        break;
                    case IntCodeOp.MULTIPLY:
                        WriteMemory(instr.GetWriteAddress(3), instr.GetParam(1) * instr.GetParam(2));
                        break;
                    case IntCodeOp.INPUT:
                        if (Inputs.Count == 0)
                        {
                            return HaltType.HALT_WAITING;
                        }

                        WriteMemory(instr.GetWriteAddress(1), Inputs.Dequeue());

                        break;
                    case IntCodeOp.OUTPUT:
                        Outputs.Enqueue(instr.GetParam(1));
                        break;
                    case IntCodeOp.JUMP_TRUE:
                        if (instr.GetParam(1) != 0)
                        {
                            IP = instr.GetParam(2);
                            IPModified = true;
                        }
                        break;
                    case IntCodeOp.JUMP_FALSE:
                        if (instr.GetParam(1) == 0)
                        {
                            IP = instr.GetParam(2);
                            IPModified = true;
                        }
                        break;
                    case IntCodeOp.LT:
                        WriteMemory(instr.GetWriteAddress(3), instr.GetParam(1) < instr.GetParam(2) ? 1 : 0);
                        break;
                    case IntCodeOp.EQUALS:
                        WriteMemory(instr.GetWriteAddress(3), instr.GetParam(1) == instr.GetParam(2) ? 1 : 0);
                        break;
                    case IntCodeOp.ADJUST_REL_BASE:
                        RelativeBase += instr.GetParam(1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (IPModified == false)
                {
                    IP += instr.Length;
                }
            }
        }
        private void ResizeMemory(long size)
        {
            var newMem = new long[size];
            memory!.CopyTo(newMem, 0);
            memory = newMem;
        }
        public long ReadMemory(long x)
        {
            if (x > memory!.Length - 1)
            {
                ResizeMemory(x + 1);
            }
            return memory[x];
        }

        public void WriteMemory(long x, long value)
        {
            if (x > memory!.Length - 1)
            {
                ResizeMemory(x + 1);
            }
            if (x >= 0)
            {
                memory[x] = value;
            }
        }
    }
}
