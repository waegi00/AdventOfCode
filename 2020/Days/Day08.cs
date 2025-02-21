using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        return new Handheld(this.InputToLines())
            .Run()
            .ToString();
    }

    public string SolveSecond()
    {
        return new Handheld(this.InputToLines())
            .Run(true)
            .ToString();
    }

    private class Handheld(string[] code)
    {
        private readonly Instruction[] originalBootCode = code.Select(Instruction.Parse).ToArray();
        private readonly HashSet<long> seenPcs = [];
        private long accumulator;
        private long pc;
        private bool exited;


        public long Run(bool part2 = false)
        {
            var mutablePcs = new Stack<long>();

            if (!part2)
            {
                return RunInternal(originalBootCode);
            }

            foreach (var (_, i) in originalBootCode.Select((x, i) => (x, i)).Where(x => x.x.operation is Operation.Jmp or Operation.Nop))
            {
                mutablePcs.Push(i);
            }

            while (mutablePcs.TryPop(out var index) && !exited)
            {
                Reset();

                var copiedBootCode = originalBootCode.ToArray();
                var curr = copiedBootCode[index].operation;
                copiedBootCode[index] = copiedBootCode[index] with
                {
                    operation = curr == Operation.Nop ? Operation.Jmp : Operation.Nop
                };
                RunInternal(copiedBootCode);
            }

            return accumulator;

        }

        private long RunInternal(Instruction[] bootCode)
        {
            while (bootCode.IsValidPosition(pc) && seenPcs.Add(pc))
            {
                Handle(bootCode[pc]);
            }

            exited = !bootCode.IsValidPosition(pc);

            return accumulator;
        }

        private void Handle(Instruction instruction)
        {
            switch (instruction.operation)
            {
                case Operation.Acc:
                    accumulator += instruction.value;
                    pc++;
                    break;
                case Operation.Jmp:
                    pc += instruction.value;
                    break;
                case Operation.Nop:
                    pc++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(instruction), "Operation of instruction is unknown");
            }
        }
        private void Reset()
        {
            seenPcs.Clear();
            accumulator = 0;
            pc = 0;
            exited = false;
        }

        private record Instruction(Operation operation, long value)
        {
            public static Instruction Parse(string line)
            {
                var splits = line.Split(' ');
                return new Instruction((Operation)Enum.Parse(typeof(Operation), splits[0], true), int.Parse(splits[1]));
            }
        }

        private enum Operation
        {
            Acc = 0,
            Jmp = 1,
            Nop = 2
        }
    }
}