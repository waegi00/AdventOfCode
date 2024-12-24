using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2024.Days;

public class Day24 : IRiddle
{
    private readonly Dictionary<string, bool> _cache = new();

    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .ToList();

        _cache.Clear();

        var instructions = new Queue<Instruction>();

        foreach (var line in input[0])
        {
            var parts = line.Split(": ");
            _cache.Add(parts[0], parts[1] == "1");
        }

        foreach (var line in input[1])
        {
            var parts = line.Split(" -> ");
            var splits = parts[0].Split(' ');
            instructions.Enqueue(
                new Instruction(
                    splits[0],
                    splits[2],
                    ToOperation(splits[1]),
                    parts[1]));
        }

        while (instructions.Count > 0)
        {
            var curr = instructions.Dequeue();
            if (_cache.ContainsKey(curr.A) && _cache.ContainsKey(curr.B))
            {
                _cache[curr.Destination] = Evaluate(curr);
            }
            else
            {
                instructions.Enqueue(curr);
            }
        }

        return _cache
            .Where(x => x.Value && x.Key.StartsWith('z'))
            .Aggregate(0L, (current, z) => current | (z.Value ? 1L << int.Parse(z.Key[1..]) : 0))
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .ToList();

        var gates = new Dictionary<string, Instruction2>();

        foreach (var line in input[1])
        {
            var parts = line.Split(" -> ");
            var splits = parts[0].Split(' ');
            gates[parts[1]] = new Instruction2(splits[0], splits[2], ToOperation(splits[1]));
        }

        var swaps = new List<string>();
        var gateAnd = new string[45];
        var gateXor = new string[45];
        var gateZ = new string[45];
        var gateTmp = new string[45];
        var gateCarry = new string[45];

        var i = 0;
        string x = $"x{i:D2}", y = $"y{i:D2}";
        gateAnd[i] = FindRule(x, Operation.And, y);
        gateXor[i] = FindRule(x, Operation.Xor, y);
        gateZ[i] = gateXor[i];
        gateCarry[i] = gateAnd[i];

        for (i = 1; i < 45; i++)
        {
            x = $"x{i:D2}";
            y = $"y{i:D2}";
            var z = $"z{i:D2}";

            var check = true;
            while (check)
            {
                check = false;

                gateAnd[i] = FindRule(x, Operation.And, y);
                gateXor[i] = FindRule(x, Operation.Xor, y);

                var instruction = gates[z];
                if (instruction.A == gateCarry[i - 1] && instruction.B != gateXor[i])
                {
                    Swap(instruction.B, gateXor[i]);
                    check = true;
                    continue;
                }
                if (instruction.B == gateCarry[i - 1] && instruction.A != gateXor[i])
                {
                    Swap(instruction.A, gateXor[i]);
                    check = true;
                    continue;
                }

                gateZ[i] = FindRule(gateXor[i], Operation.Xor, gateCarry[i - 1]);
                if (gateZ[i] != z)
                {
                    Swap(gateZ[i], z);
                    check = true;
                    continue;
                }

                gateTmp[i] = FindRule(gateXor[i], Operation.And, gateCarry[i - 1]);
                gateCarry[i] = FindRule(gateTmp[i], Operation.Or, gateAnd[i]);
            }
        }

        return string.Join(",", swaps.Order());

        void Swap(string wire1, string wire2)
        {
            (gates[wire1], gates[wire2]) = (gates[wire2], gates[wire1]);
            swaps.Add(wire1);
            swaps.Add(wire2);
        }

        string FindRule(string wire1, Operation operation, string wire2)
        {
            foreach (var kvp in gates)
            {
                var instruction = kvp.Value;
                if (operation == instruction.Operation && (wire1 == instruction.A && wire2 == instruction.B || wire1 == instruction.B && wire2 == instruction.A))
                {
                    return kvp.Key;
                }
            }

            throw new Exception("Can't happen");
        }
    }

    private bool Evaluate(Instruction instruction)
    {
        return instruction.Operation switch
        {
            Operation.And => _cache[instruction.A] && _cache[instruction.B],
            Operation.Or => _cache[instruction.A] || _cache[instruction.B],
            Operation.Xor => _cache[instruction.A] ^ _cache[instruction.B],
            _ => throw new ArgumentOutOfRangeException(nameof(instruction))
        };
    }

    private static Operation ToOperation(string str)
    {
        return str switch
        {
            "AND" => Operation.And,
            "OR" => Operation.Or,
            "XOR" => Operation.Xor,
            _ => throw new ArgumentOutOfRangeException(nameof(str))
        };
    }

    private enum Operation { And, Or, Xor, }
    private record Instruction(string A, string B, Operation Operation, string Destination);
    private record Instruction2(string A, string B, Operation Operation);
}