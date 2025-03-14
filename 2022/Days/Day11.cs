using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var monkeys = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x =>
            {
                var lines = x.Split("\r\n");
                return new Monkey(
                    lines[1].Split(':')[1].Split(',').Select(long.Parse).ToList(),
                    lines[2].Split("= ")[1].Split(' '),
                    (int.Parse(lines[3].Split(' ')[^1]), int.Parse(lines[4].Split(' ')[^1]), int.Parse(lines[5].Split(' ')[^1])));
            })
            .ToArray();

        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect(monkeys);
            }
        }

        return monkeys.Select(x => x.Inspected)
            .OrderDescending()
            .Take(2)
            .Product()
            .ToString();
    }

    public string SolveSecond()
    {
        var monkeys = this.InputToText()
            .Split("\r\n\r\n")
            .Select(x =>
            {
                var lines = x.Split("\r\n");
                return new Monkey(
                    lines[1].Split(':')[1].Split(',').Select(long.Parse).ToList(),
                    lines[2].Split("= ")[1].Split(' '),
                    (long.Parse(lines[3].Split(' ')[^1]), int.Parse(lines[4].Split(' ')[^1]), int.Parse(lines[5].Split(' ')[^1])),
                    true);
            })
            .ToArray();

        var div = monkeys.Select(m => m.DivisionRule.div).Product();

        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect(monkeys, div);
            }
        }

        return monkeys.Select(x => x.Inspected)
            .OrderDescending()
            .Take(2)
            .Product()
            .ToString();
    }

    private class Monkey(List<long> items, string[] operation, (long div, int t, int f) divisionRule, bool isPart2 = false)
    {
        public long Inspected { get; private set; }

        private List<long> Items { get; } = items;

        private string[] Operation { get; } = operation;

        public (long div, int t, int f) DivisionRule { get; } = divisionRule;

        public void Inspect(Monkey[] monkeys, long? div = null)
        {
            var l = Items.Count;
            for (var i = 0; i < l; i++)
            {
                var item = Result(Items[0]);
                if (!isPart2)
                {
                    item /= 3;
                }

                if (div != null)
                {
                    item %= div.Value;
                }
                monkeys[item % DivisionRule.div == 0 ? DivisionRule.t : DivisionRule.f].Items.Add(item);
                Items.RemoveAt(0);
                Inspected++;
            }
        }

        private long Result(long input)
        {
            var a = Number(Operation[0], input);
            var b = Number(Operation[2], input);

            return Operation[1] switch
            {
                "*" => a * b,
                "+" => a + b,
                _ => throw new Exception()
            };
        }

        private static long Number(string input, long curr)
        {
            if (input == "old") return curr;
            if (long.TryParse(input, out var result)) return result;
            throw new Exception();
        }
    }
}