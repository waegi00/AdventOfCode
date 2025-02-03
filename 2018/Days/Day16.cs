using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day16 : IRiddle
{
    private readonly List<string> ops =
    [
        "addr", "addi", "mulr", "muli",
        "banr", "bani", "borr", "bori",
        "setr", "seti", "gtir", "gtri",
        "gtrr", "eqir", "eqri", "eqrr",
    ];

    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n\r\n")[0]
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .ToArray();

        var sum = 0;
        foreach (var sample in input)
        {
            var curr = 0;

            var before = sample[0][9..^1].Split(", ").Select(int.Parse).ToArray();
            var after = sample[2][9..^1].Split(", ").Select(int.Parse).ToArray();

            var data = sample[1].Split(' ').Select(int.Parse).ToArray();
            foreach (var op in ops)
            {
                var registers = before.ToArray();

                Apply(registers, op, data[1], data[2], data[3]);

                if (!registers.SequenceEqual(after) || ++curr != 3)
                {
                    continue;
                }

                sum++;
                break;

            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n\r\n")
            .ToArray();

        var samples = input[0]
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .ToArray();

        var opCodes = Enumerable.Range(0, ops.Count)
            .Select(x => (x, ops: ops.ToList()))
            .ToDictionary(x => x.x, x => x.ops);

        foreach (var sample in samples)
        {
            var before = sample[0][9..^1].Split(", ").Select(int.Parse).ToArray();
            var after = sample[2][9..^1].Split(", ").Select(int.Parse).ToArray();

            var data = sample[1].Split(' ').Select(int.Parse).ToArray();
            foreach (var op in opCodes[data[0]].ToList())
            {
                var registers = before.ToArray();

                Apply(registers, op, data[1], data[2], data[3]);

                if (!registers.SequenceEqual(after))
                {
                    opCodes[data[0]].Remove(op);
                }
            }
        }

        var map = new Dictionary<int, string>();

        while (opCodes.Count != 0)
        {
            var (k, value) = opCodes.First(x => x.Value.Count == 1);
            var item = value.First();
            map.Add(k, item);
            opCodes.Remove(k);

            foreach (var (otherK, _) in opCodes)
            {
                opCodes[otherK].Remove(item);
            }
        }

        var res = new int[4];

        foreach (var instruction in input[1].Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries))
        {
            var data = instruction.Split(' ').Select(int.Parse).ToArray();
            Apply(res, map[data[0]], data[1], data[2], data[3]);
        }

        return res[0].ToString();
    }

    private static void Apply(int[] registers, string op, int a, int b, int c)
    {
        registers[c] = op switch
        {
            "addr" => registers[a] + registers[b],
            "addi" => registers[a] + b,
            "mulr" => registers[a] * registers[b],
            "muli" => registers[a] * b,
            "banr" => registers[a] & registers[b],
            "bani" => registers[a] & b,
            "borr" => registers[a] | registers[b],
            "bori" => registers[a] | b,
            "setr" => registers[a],
            "seti" => a,
            "gtir" => a > registers[b] ? 1 : 0,
            "gtri" => registers[a] > b ? 1 : 0,
            "gtrr" => registers[a] > registers[b] ? 1 : 0,
            "eqir" => a == registers[b] ? 1 : 0,
            "eqri" => registers[a] == b ? 1 : 0,
            "eqrr" => registers[a] == registers[b] ? 1 : 0,
            _ => registers[c]
        };
    }
}