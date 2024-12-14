using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day22 : IRiddle
{
    private static readonly Dictionary<string, int> SpellList = new()
    {
        { "Magic Missile", 53 },
        { "Drain", 73 },
        { "Shield", 113 },
        { "Poison", 173 },
        { "Recharge", 229 }
    };

    public string SolveFirst()
    {
        var input = this.InputToLines().Select(x => int.Parse(x.Split(": ")[1])).ToList();
        var (bossHp, bossDamage) = (input[0], input[1]);

        var min = new State(int.MaxValue, 0, 0, 0, [], false, []);

        var queue = new Queue<State>();
        queue.Enqueue(new State(0, bossHp, 50, 500, [], true, []));
        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (curr.ManaSpent >= min.ManaSpent) continue;
            if (curr.BossHp <= 0)
            {
                min = curr;
                continue;
            }
            if (curr.Hp <= 0) continue;

            if (curr.PlayersTurn)
            {
                foreach (var spell in SpellList)
                {
                    if (spell.Value > curr.Mana) continue;
                    if (curr.Effects.ContainsKey(spell.Key)) continue;

                    queue.Enqueue(curr.Transfer(spell.Key, 0));
                }
            }
            else
            {
                queue.Enqueue(curr.Transfer(string.Empty, bossDamage));
            }
        }

        return min.ManaSpent.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().Select(x => int.Parse(x.Split(": ")[1])).ToList();
        var (bossHp, bossDamage) = (input[0], input[1]);

        var min = new State(int.MaxValue, 0, 0, 0, [], false, []);

        var queue = new Queue<State>();
        queue.Enqueue(new State(0, bossHp, 50, 500, [], true, []));
        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (curr.ManaSpent >= min.ManaSpent) continue;
            if (curr.BossHp <= 0)
            {
                min = curr;
                continue;
            }
            if (curr.PlayersTurn)
            {
                var newHp = curr.Hp - 1;
                curr = curr with { Hp = newHp };
            }
            if (curr.Hp <= 0) continue;

            if (curr.PlayersTurn)
            {
                foreach (var spell in SpellList)
                {
                    if (spell.Value > curr.Mana) continue;
                    if (curr.Effects.ContainsKey(spell.Key) && curr.Effects[spell.Key] > 1) continue;

                    queue.Enqueue(curr.Transfer(spell.Key, 0));
                }
            }
            else
            {
                queue.Enqueue(curr.Transfer(string.Empty, bossDamage));
            }
        }

        return min.ManaSpent.ToString();
    }

    private record State(int ManaSpent, int BossHp, int Hp, int Mana, Dictionary<string, int> Effects, bool PlayersTurn, List<string> Spells)
    {
        public State Transfer(string spell, int bossDamage)
        {
            var cost = spell == string.Empty ? 0 : SpellList[spell];
            var newManaSpent = ManaSpent + cost;
            var newBossHp = BossHp;
            var newHp = Hp;
            var newMana = Mana - cost;
            var newSpells = new List<string>(Spells);
            var hasShield = false;
            if (spell != string.Empty)
            {
                newSpells.Add(spell);
            }
            var newEffects = new Dictionary<string, int>();

            foreach (var effect in Effects)
            {
                switch (effect.Key)
                {
                    case "Shield":
                        hasShield = true;
                        break;
                    case "Poison":
                        newBossHp -= 3;
                        break;
                    case "Recharge":
                        newMana += 101;
                        break;
                }

                if (effect.Value > 1)
                {
                    newEffects.Add(effect.Key, effect.Value - 1);
                }
            }

            if (!PlayersTurn)
            {
                newHp -= hasShield ? Math.Max(1, bossDamage - 7) : bossDamage;
                return new State(ManaSpent: ManaSpent, BossHp: newBossHp, Hp: newHp, Mana: newMana, Effects: newEffects, PlayersTurn: true, Spells: newSpells);
            }

            switch (spell)
            {
                case "Magic Missile":
                    newBossHp -= 4;
                    break;
                case "Drain":
                    newBossHp -= 2;
                    newHp += 2;
                    break;
                case "Shield":
                    newEffects.Add("Shield", 6);
                    break;
                case "Poison":
                    newEffects.Add("Poison", 6);
                    break;
                case "Recharge":
                    newEffects.Add("Recharge", 5);
                    break;
            }

            return new State(ManaSpent: newManaSpent, BossHp: newBossHp, Hp: newHp, Mana: newMana, Effects: newEffects, PlayersTurn: false, Spells: newSpells);
        }
    }
}