using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2022.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines()
            .Select(x =>
            {
                var splits = x.Split(' ');
                return new Instruction(splits[0], splits.Length == 2 ? long.Parse(splits[1]) : null);
            });

        var result = new CPU(new(input)).Execute();

        return result.WithIndex()
            .Where(x => (x.index + 20) % 40 == 0)
            .Sum(x => result[x.index - 1] * x.index)
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines()
            .Select(x =>
            {
                var splits = x.Split(' ');
                return new Instruction(splits[0], splits.Length == 2 ? long.Parse(splits[1]) : null);
            });

        var result = new CPU(new(input)).Execute();

        for (var r = 0; r < 6; r++)
        {
            for (var c = 0; c < 40; c++)
            {
                var x = result[r * 40 + c];
                var sprite = new[] { (x - 1).Mod(40), x.Mod(40), (x + 1).Mod(40) };

                Console.Write(sprite.Contains(c) ? '#' : ' ');
            }

            Console.WriteLine();
        }

        return "";
    }

    private class CPU(Queue<Instruction> instructions)
    {
        private readonly List<long> X = [1];

        public List<long> Execute()
        {
            while (instructions.TryDequeue(out var instruction))
            {
                Execute(instruction);
            }

            return X;
        }

        private void Execute(Instruction instruction)
        {
            switch (instruction.Operation)
            {
                case "noop":
                    Cycle();
                    break;
                case "addx":
                    Cycle(2, instruction.Value!.Value);
                    break;
            }
        }

        private void Cycle(int n = 1, long increment = 0)
        {
            for (var i = 0; i < n - 1; i++)
            {
                X.Add(X[^1]);
            }

            X.Add(X[^1] + increment);
        }
    }

    private record Instruction(string Operation, long? Value);
}