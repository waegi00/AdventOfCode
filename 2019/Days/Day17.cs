using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2019.Days;

public partial class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var memory = this.InputToText()
            .Split(',')
            .Select((x, i) => (i: (long)i, x: long.Parse(x)))
            .ToDictionary(x => x.i, x => x.x);

        var input = new Queue<long>();
        var output = RunProgram(memory, input).ToList();

        var grid = new string(output.Select(x => (char)x).ToArray())
            .Split((char)10)
            .ToCharArray();

        var sum = 0;
        for (var r = 1; r < grid.Length - 1; r++)
        {
            for (var c = 1; c < grid[r].Length - 1; c++)
            {
                if (grid[r][c] == '#' && grid.Neighbours(r, c, includeDiagonal: false)
                        .All(x => x.Item1 == '#'))
                {
                    sum += c * r;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var memory = this.InputToText()
            .Split(',')
            .Select((num, i) => (i: (long)i, x: long.Parse(num)))
            .ToDictionary(item => item.i, item => item.x);

        var output = RunProgram(memory.ToDictionary(), new Queue<long>()).ToList();

        var str = new string(output.Select(num => (char)num).ToArray());
        var grid = str
            .Split((char)10)
            .ToCharArray();

        var c = str.First("^<v>".Contains);
        var (x, y) = grid.FindFirst(c);
        var dir = Dir(c);
        var path = new List<(char, int)> { ('R', 0) };
        while (true)
        {
            var nx = x + dir.x;
            var ny = y + dir.y;

            if (grid.IsValidPosition(nx, ny) && grid[nx][ny] == '#')
            {
                x = nx;
                y = ny;
                path[^1] = (path[^1].Item1, path[^1].Item2 + 1);
            }
            else
            {
                var tryR = TurnRight(dir);
                nx = x + tryR.x;
                ny = y + tryR.y;
                if (grid.IsValidPosition(nx, ny) && grid[nx][ny] == '#')
                {
                    x = nx;
                    y = ny;
                    dir = tryR;
                    path.Add(('R', 1));
                }
                else
                {
                    var tryL = TurnLeft(dir);
                    nx = x + tryL.x;
                    ny = y + tryL.y;
                    if (grid.IsValidPosition(nx, ny) && grid[nx][ny] == '#')
                    {
                        x = nx;
                        y = ny;
                        dir = tryL;
                        path.Add(('L', 1));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        var route = new string(path.SelectMany(item => $"{item.Item1},{item.Item2},").ToArray());

        var functions = string.Empty;

        var matchGroups = GroupRegex().Match(route).Groups;
        for (var i = 1; i < matchGroups.Count; i++)
        {
            var routineName = (char)('A' - 1 + i);
            route = route.Replace(matchGroups[i].Value, routineName + ",");
            functions += matchGroups[i].Value[..^1] + "\n";
        }

        var input = new Queue<long>((route[..^1] + "\n" + functions + "n\n").ToCharArray().Select(ch => (long)ch));
        memory[0] = 2;

        return RunProgram(memory.ToDictionary(), input).Last().ToString();

        (int x, int y) TurnRight((int, int) direction) => (direction.Item2, -direction.Item1);

        (int x, int y) TurnLeft((int, int) direction) => (-direction.Item2, direction.Item1);

        (int x, int y) Dir(char character) => character switch
        {
            '^' => (0, 1),
            'v' => (0, -1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new Exception()
        };
    }

    private static IEnumerable<long> RunProgram(Dictionary<long, long> memory, Queue<long> input)
    {
        long relBase = 0, pc = 0;
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

    [GeneratedRegex("^(.{1,21})\\1*(.{1,21})(?:\\1|\\2)*(.{1,21})(?:\\1|\\2|\\3)*$")]
    private static partial Regex GroupRegex();
}