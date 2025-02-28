using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day19 : IRiddle
{
    public string SolveFirst()
    {
        return Calculate(this.InputToLines(), true)
            .ToString();
    }

    public string SolveSecond()
    {
        return Calculate(this.InputToLines(), false)
            .ToString();
    }

    private static int Calculate(string[] input, bool isPart1)
    {
        var unresolved = new List<(int, List<string>)>();
        var messages = new List<string>();
        var c42 = new List<string>();
        var c31 = new List<string>();

        var resolved = new Queue<(int, List<string>)>();

        foreach (var bits in input.Select(x => x.Split(':', '|')))
        {
            if (bits.Length == 1)
            {
                messages.Add(bits[0]);
            }
            else
            {
                var ruleNo = int.Parse(bits[0]);
                var rule1 = bits[1];
                if (bits.Length == 2)
                {
                    if (!rule1.Any(char.IsDigit))
                    {
                        resolved.Enqueue((ruleNo, [rule1.Replace("\"", "").Trim()]));
                    }
                    else
                    {
                        unresolved.Add((ruleNo, [rule1 + ' ']));
                    }
                }
                else
                {
                    unresolved.Add((ruleNo, [rule1 + ' ', bits[2] + ' ']));
                }
            }
        }


        while (resolved.Count > 0)
        {
            var (ruleNo, combos) = resolved.Dequeue();
            for (var i = 0; i < unresolved.Count; i++)
            {
                var changeMade = false;
                var (partial, rules) = unresolved[i];
                if (rules.Count == 0) continue;

                var pattern = ' ' + ruleNo.ToString() + ' ';
                while (rules.Any(r => r.Contains(pattern)))
                {
                    var newList = new List<string>();
                    foreach (var rule in rules)
                    {
                        if (!rule.Contains(pattern))
                        {
                            newList.Add(rule);
                        }
                        else
                        {
                            var pos = rule.IndexOf(pattern, StringComparison.Ordinal);
                            newList.AddRange(combos.Select(combo => rule[..pos] + " " + combo + rule[(pos + pattern.Length - 1)..]).Where(newRule => newRule.Any(char.IsDigit) || partial != 0));
                            changeMade = true;
                        }
                    }
                    rules = newList.ToList();
                }

                if (!rules.Any(s => s.Any(char.IsDigit)))
                {
                    rules = rules.Select(s => s.Replace(" ", "")).ToList();
                    switch (partial)
                    {
                        case 42:
                            c42 = rules;
                            break;
                        case 31:
                            c31 = rules;
                            break;
                        default:
                            resolved.Enqueue((partial, rules));
                            break;
                    }
                    unresolved[i] = (partial, []);
                }
                else if (changeMade)
                    unresolved[i] = (partial, rules);
            }
            unresolved.RemoveAll(t => t.Item2.Count == 0);
        }

        return messages.Count(m => isValid(m, c42, c31, isPart1));
    }

    private static bool isValid(string message, List<string> c42, List<string> c31, bool isPart1)
    {
        var len = c42[0].Length;
        var slices = message.Length / len;
        var max31s = (slices - 1) / 2;

        if (message.Length % len != 0 ||
            (isPart1 && message.Length != len * 3) ||
            (message.Length >= len && !c42.Contains(message[..len])) ||
            (message.Length >= len && !c31.Contains(message[^len..])))
        {
            return false;
        }

        if (isPart1)
        {
            return c42.Contains(message[len..(2 * len)]);
        }

        var v42 = new int[slices];
        var v31 = new int[slices];
        for (var i = 0; i < slices; i++)
        {
            var slice = message[(i * len)..((i + 1) * len)];
            if (c42.Contains(slice))
            {
                v42[i] = 1;
            }

            if (c31.Contains(slice))
            {
                v31[i] = 1;
            }
        }

        for (var n = 1; n <= max31s; n++)
        {
            if (v31[^n..].Sum() + v42[..(slices - n)].Sum() == slices)
            {
                return true;
            }
        }

        return false;
    }
}