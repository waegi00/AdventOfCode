using AdventOfCode.Interfaces;
using System.Text.RegularExpressions;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public partial class Day24 : IRiddle
{
    public string SolveFirst()
    {
        return Fight(this.InputToLines(), 0).units.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var l = 0;
        var h = int.MaxValue / 2;
        while (h - l > 1)
        {
            var m = (h + l) / 2;
            if (Fight(input, m).immuneSystem)
            {
                h = m;
            }
            else
            {
                l = m;
            }
        }
        return Fight(input, h).units.ToString();
    }

    private static (bool immuneSystem, int units) Fight(string[] lines, int b)
    {
        var army = Parse(lines);
        foreach (var g in army.Where(g => g.immuneSystem))
        {
            g.damage += b;
        }
        var attack = true;
        while (attack)
        {
            attack = false;
            var remainingTarget = new HashSet<Group>(army);
            var targets = new Dictionary<Group, Group>();
            foreach (var g in army.OrderByDescending(g => (g.effectivePower, g.initiative)))
            {
                var maxDamage = remainingTarget.Select(t => g.DamageGivenTo(t)).Max();
                if (maxDamage <= 0) continue;

                var possibleTargets = remainingTarget.Where(t => g.DamageGivenTo(t) == maxDamage);
                targets[g] = possibleTargets.OrderByDescending(t => (t.effectivePower, t.initiative)).First();
                remainingTarget.Remove(targets[g]);

            }
            foreach (var g in targets.Keys.OrderByDescending(g => g.initiative))
            {
                if (g.units <= 0) continue;

                var target = targets[g];
                var damage = g.DamageGivenTo(target);
                if (damage <= 0 || target.units <= 0) continue;

                var dies = damage / target.hp;
                target.units = Math.Max(0, target.units - dies);
                if (dies > 0)
                {
                    attack = true;
                }
            }
            army = army.Where(g => g.units > 0).ToList();
        }
        return (army.All(x => x.immuneSystem), army.Select(x => x.units).Sum());
    }

    private static List<Group> Parse(string[] lines)
    {
        var immuneSystem = false;
        var res = new List<Group>();
        foreach (var line in lines)
            switch (line)
            {
                case "Immune System:":
                    immuneSystem = true;
                    break;
                case "Infection:":
                    immuneSystem = false;
                    break;
                default:
                    {
                        if (line != "")
                        {
                            var m = GroupRegex().Match(line);
                            if (m.Success)
                            {
                                var g = new Group
                                {
                                    immuneSystem = immuneSystem,
                                    units = int.Parse(m.Groups[1].Value),
                                    hp = int.Parse(m.Groups[2].Value),
                                    damage = int.Parse(m.Groups[4].Value),
                                    attackType = m.Groups[5].Value.Trim(),
                                    initiative = int.Parse(m.Groups[6].Value)
                                };
                                var st = m.Groups[3].Value.Trim();
                                if (st != "")
                                {
                                    st = st.Substring(1, st.Length - 2);
                                    foreach (var part in st.Split(";"))
                                    {
                                        var k = part.Split(" to ");
                                        var set = new HashSet<string>(k[1].Split(", "));
                                        var w = k[0].Trim();
                                        switch (w)
                                        {
                                            case "immune":
                                                g.immuneTo = set;
                                                break;
                                            case "weak":
                                                g.weakTo = set;
                                                break;
                                            default:
                                                throw new Exception();
                                        }
                                    }
                                }
                                res.Add(g);
                            }
                            else
                            {
                                throw new Exception();
                            }

                        }

                        break;
                    }
            }
        return res;
    }

    private class Group
    {
        public bool immuneSystem;
        public int units;
        public int hp;
        public int damage;
        public int initiative;
        public string attackType;
        public HashSet<string> immuneTo = [];
        public HashSet<string> weakTo = [];

        public int effectivePower => units * damage;

        public int DamageGivenTo(Group target)
        {
            if (target.immuneSystem == immuneSystem)
            {
                return 0;
            }

            if (target.immuneTo.Contains(attackType))
            {
                return 0;
            }

            if (target.weakTo.Contains(attackType))
            {
                return effectivePower * 2;
            }

            return effectivePower;
        }
    }

    [GeneratedRegex(@"(\d+) units each with (\d+) hit points(.*)with an attack that does (\d+)(.*)damage at initiative (\d+)")]
    private static partial Regex GroupRegex();
}