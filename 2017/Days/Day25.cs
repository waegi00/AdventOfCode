using System.ComponentModel.DataAnnotations;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2017.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x => x.Split("\r\n"))
            .ToArray();

        var n = int.Parse(input[0][1].Split(' ')[^2]);
        var curr = input[0][0][^2];
        var states = new List<State>();
        foreach (var item in input[1..])
        {
            var name = item[0][^2];
            var instructions = new Instruction[2];
            for (var i = 0; i < 2; i++)
            {
                var write = item[2 + i * 4][^2] - '0';
                var moveRight = item[3 + i * 4][^3] == 'h';
                var next = item[4 + i * 4][^2];
                instructions[i] = new Instruction(write, moveRight, next);
            }
            states.Add(new State(name, instructions[0], instructions[1]));
        }

        var tape = new HashSet<int>();
        var pos = 0;

        while (n-- > 0)
        {
            var state = states.First(x => x.Name == curr);
            var instruction = tape.Contains(pos) ? state.One : state.Zero;

            _ = instruction.Write == 1 ? tape.Add(pos) : tape.Remove(pos);
            pos += instruction.MoveRight ? 1 : -1;
            curr = instruction.Next;
        }

        return tape.Count.ToString();
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }

    private record State(char Name, Instruction Zero, Instruction One);

    private record Instruction(int Write, bool MoveRight, char Next);
}