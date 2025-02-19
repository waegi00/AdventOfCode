using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var d = new Droid(input);
        d.ExploreFully();

        return d.map.PathLength(d.leak!.Value).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(long.Parse)
            .ToList();

        var d = new Droid(input);
        d.ExploreFully();

        return d.map.GetMaxDistance(d.leak!.Value).ToString();
    }

    private class IntCode
    {
        private readonly List<long> inputQueue = [];

        public long output;
        private readonly IntCode? outputDest = null;

        private readonly Dictionary<long, long> memory;
        private long ip;
        private long rb;

        private bool waiting;
        private bool halted;
        private bool sentOutput;

        private readonly int id;
        private static int nextID;

        public override string ToString() => "IntCode" + id;

        public IntCode(List<long> initialMemory)
        {
            id = nextID;
            nextID++;
            memory = new Dictionary<long, long>();
            long address = 0;
            foreach (var n in initialMemory)
            {
                memory[address] = n;
                address = address + 1;
            }
        }

        public void SendInput(long input)
        {
            inputQueue.Add(input);
            waiting = false;
        }

        private void Output(long i)
        {
            output = i;
            outputDest?.SendInput(i);
            sentOutput = true;
        }

        private long GetMemory(long address) => memory.GetValueOrDefault(address, 0);

        private void SetMemory(long address, long value)
        {
            memory[address] = value;
        }

        private int AccessMode(int idx)
        {
            var mode = (int)GetMemory(ip) / 100;
            for (var i = 1; i < idx; i++)
            {
                mode /= 10;
            }
            return mode % 10;
        }

        private void SetParam(int idx, long value)
        {
            var param = GetMemory(ip + idx);
            switch (AccessMode(idx))
            {
                case 0:
                    SetMemory(param, value);
                    break;
                case 2:
                    SetMemory(rb + param, value);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private long GetParam(int idx)
        {
            var param = GetMemory(ip + idx);
            return AccessMode(idx) switch
            {
                0 => GetMemory(param),
                1 => param,
                2 => GetMemory(rb + param),
                _ => throw new ArgumentException()
            };
        }

        private void Step()
        {
            var opcode = (int)(GetMemory(ip) % 100);
            switch (opcode)
            {
                case 1:
                    SetParam(3, GetParam(1) + GetParam(2));
                    ip += 4;
                    break;
                case 2:
                    SetParam(3, GetParam(1) * GetParam(2));
                    ip += 4;
                    break;
                case 3:
                    if (inputQueue.Count > 0)
                    {
                        SetParam(1, inputQueue[0]);
                        inputQueue.RemoveAt(0);
                        ip += 2;
                    }
                    else
                    {
                        waiting = true;
                    }
                    break;
                case 4:
                    Output(GetParam(1));
                    ip += 2;
                    break;
                case 5:
                    if (GetParam(1) == 0)
                        ip += 3;
                    else
                        ip = GetParam(2);
                    break;
                case 6:
                    if (GetParam(1) == 0)
                        ip = GetParam(2);
                    else
                        ip += 3;
                    break;
                case 7:
                    SetParam(3, GetParam(1) < GetParam(2) ? 1 : 0);
                    ip += 4;
                    break;
                case 8:
                    SetParam(3, GetParam(1) == GetParam(2) ? 1 : 0);
                    ip += 4;
                    break;
                case 9:
                    rb += GetParam(1);
                    ip += 2;
                    break;
                case 99:
                    halted = true;
                    break;
                default:
                    throw new ArgumentException();
            }

        }

        public void RunToOutput()
        {
            sentOutput = false;
            while (!halted && !waiting && !sentOutput)
            {
                Step();
            }
        }
    }

    private class Map(Dictionary<(int, int), int> course)
    {
        private readonly Dictionary<(int, int), bool> Walls = new();
        private Dictionary<(int, int), int> Course = course;
        public readonly List<(int, int)> Frontier = [];

        private static readonly (int, int) origin = new(0, 0);

        public static (int, int) NorthOf((int, int) p)
        {
            return (p.Item1, p.Item2 + 1);
        }

        public static (int, int) SouthOf((int, int) p)
        {
            return (p.Item1, p.Item2 - 1);
        }

        public static (int, int) EastOf((int, int) p)
        {
            return (p.Item1 + 1, p.Item2);
        }

        public static (int, int) WestOf((int, int) p)
        {
            return (p.Item1 - 1, p.Item2);
        }

        private void AddFrontierMaybe((int, int) pos)
        {
            if (!Walls.ContainsKey(pos) && !Frontier.Contains(pos))
                Frontier.Add(pos);
        }

        public void AddWall((int, int) pos)
        {
            Frontier.Remove(pos);
            Walls.TryAdd(pos, true);
        }
        public void AddClear((int, int) pos)
        {
            Frontier.Remove(pos);
            if (!Walls.TryAdd(pos, false)) return;
            AddFrontierMaybe(NorthOf(pos));
            AddFrontierMaybe(SouthOf(pos));
            AddFrontierMaybe(EastOf(pos));
            AddFrontierMaybe(WestOf(pos));
        }

        private void AddMaybe(List<(int, int)> stuff, (int, int) p)
        {
            if (Walls.TryGetValue(p, out var value) && !value && !Course.ContainsKey(p) && !stuff.Contains(p))
            {
                stuff.Add(p);
            }
        }

        private void ChartCourse((int, int)? start, (int, int) end)
        {
            Course = new Dictionary<(int, int), int>();

            List<(int, int)> generation = [end];
            var distance = 0;

            while (generation.Count > 0)
            {
                List<(int, int)> next = [];
                foreach (var p in generation)
                {
                    Course[p] = distance;
                    Course[p] = distance;
                    if (start != null && p.Item1 == start.Value.Item1 && p.Item2 == start.Value.Item2) return;
                    AddMaybe(next, NorthOf(p));
                    AddMaybe(next, SouthOf(p));
                    AddMaybe(next, EastOf(p));
                    AddMaybe(next, WestOf(p));
                }
                generation = next;
                distance++;
            }
        }

        public int GetMaxDistance((int, int) p)
        {
            ChartCourse(null, p);
            return Course.Values.Prepend(0).Max();
        }

        public IEnumerable<int> FindPath((int, int) start, (int, int) end)
        {
            ChartCourse(start, end);
            var p = start;
            while (Course[p] != 0)
            {
                var dist = Course[p] - 1;
                var q = NorthOf(p);
                if (Course.TryGetValue(q, out var value1) && value1 == dist) yield return 1;
                else
                {
                    q = SouthOf(p);
                    if (Course.TryGetValue(q, out var value2) && value2 == dist) yield return 2;
                    else
                    {
                        q = WestOf(p);
                        if (Course.TryGetValue(q, out var value3) && value3 == dist) yield return 3;
                        else
                        {
                            q = EastOf(p);
                            if (Course.TryGetValue(q, out var value4) && value4 == dist) yield return 4;
                        }
                    }
                }
                p = q;
            }
        }

        public int PathLength((int, int) end)
        {
            ChartCourse(origin, end);
            return Course[origin];
        }

    }

    private class Droid
    {
        private readonly IntCode brain;
        private (int, int) pos;
        public readonly Map map;
        public (int, int)? leak;

        public Droid(List<long> code)
        {
            brain = new IntCode(code);
            pos = (0, 0);
            this.map = new Map([]);
            map.AddClear(pos);
            leak = null;
        }

        private void Step(int direction)
        {
            brain.SendInput(direction);
            var dest = direction switch
            {
                1 => Map.NorthOf(pos),
                2 => Map.SouthOf(pos),
                3 => Map.WestOf(pos),
                _ => Map.EastOf(pos)
            };
            brain.RunToOutput();
            if (brain.output == 0)
            {
                map.AddWall(dest);
                return;
            }
            map.AddClear(dest);
            pos = dest;
            if (brain.output == 2)
            {
                leak = dest;
            }
        }

        private void GoTo((int, int) p)
        {
            foreach (var dir in map.FindPath(pos, p))
            {
                Step(dir);
            }
        }

        public void ExploreFully()
        {
            while (map.Frontier.Count > 0)
            {
                var p = map.Frontier[0];
                map.Frontier.RemoveAt(0);
                GoTo(p);
            }
        }
    }
}