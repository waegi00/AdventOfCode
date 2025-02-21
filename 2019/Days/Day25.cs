using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var solution = new List<string>
        {
            "south\n", "take mutex\n",
            "south\n", "west\n", "west\n", "take klein bottle\n",
            "east\n", "east\n", "north\n", "east\n", "take mug\n",
            "east\n", "north\n", "north\n", "take hypercube\n",
            "south\n", "south\n", "east\n", "east\n", "east\n", "south\n", "west\n", "west\n", "west\n"
        };

        var memory = this.InputToText()
            .Split(',')
            .Select((x, i) => (i: (long)i, x: long.Parse(x)))
            .ToDictionary(x => x.i, x => x.x);

        var computer = new Computer(memory);

        Console.WriteLine("Play (y) or show solution (n)?");
        var play = Console.ReadKey().KeyChar == 'y';

        while (true)
        {
            if (!play)
            {
                computer.AddInput(solution.Select(x => x.ToCharArray()).SelectMany(x => x).Select(x => (long)x));
                Console.WriteLine(string.Join("", computer.RunProgram().Select(x => (char)x)));
                break;
            }

            var output = computer.RunProgram();
            Console.WriteLine(string.Join("", output.Select(x => (char)x)));

            var input = Console.ReadLine();
            if (input is null or "exit")
            {
                break;
            }

            computer.AddInput(input.ToCharArray().Select(x => (long)x).Append(10).ToList());
        }

        return "11534338";
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }

    private class Computer
    {
        private readonly Dictionary<long, long> memory;
        private readonly Queue<long> input = [];
        private long relBase;
        private long pc;

        public Computer(Dictionary<long, long> memory)
        {
            this.memory = memory;
        }

        private void AddInput(long l)
        {
            input.Enqueue(l);
        }

        public void AddInput(IEnumerable<long> ls)
        {
            foreach (var l in ls)
            {
                AddInput(l);
            }
        }

        public IEnumerable<long> RunProgram()
        {
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
}