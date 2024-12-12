using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day22 : IRiddle
{
    private static readonly Dictionary<string, int> spellList = new()
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
        var (bossHP, bossDamage) = (input[0], input[1]);

        var min = new State(int.MaxValue, 0, 0, 0, [], false, []);

        var queue = new Queue<State>();
        queue.Enqueue(new State(0, bossHP, 50, 500, [], true, []));
        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (curr.manaSpent >= min.manaSpent) continue;
            if (curr.bossHp <= 0)
            {
                min = curr;
                continue;
            }
            if (curr.hp <= 0) continue;

            if (curr.playersTurn)
            {
                foreach (var spell in spellList)
                {
                    if (spell.Value > curr.mana) continue;
                    if (curr.effects.ContainsKey(spell.Key)) continue;

                    queue.Enqueue(curr.Transfer(spell.Key, 0));
                }
            }
            else
            {
                queue.Enqueue(curr.Transfer(string.Empty, bossDamage));
            }
        }

        return min.manaSpent.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().Select(x => int.Parse(x.Split(": ")[1])).ToList();
        var (bossHP, bossDamage) = (input[0], input[1]);

        var min = new State(int.MaxValue, 0, 0, 0, [], false, []);

        var queue = new Queue<State>();
        queue.Enqueue(new State(0, bossHP, 50, 500, [], true, []));
        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (curr.manaSpent >= min.manaSpent) continue;
            if (curr.bossHp <= 0)
            {
                min = curr;
                continue;
            }
            if (curr.playersTurn)
            {
                var newHp = curr.hp - 1;
                curr = curr with { hp = newHp };
            }
            if (curr.hp <= 0) continue;

            if (curr.playersTurn)
            {
                foreach (var spell in spellList)
                {
                    if (spell.Value > curr.mana) continue;
                    if (curr.effects.ContainsKey(spell.Key) && curr.effects[spell.Key] > 1) continue;

                    queue.Enqueue(curr.Transfer(spell.Key, 0));
                }
            }
            else
            {
                queue.Enqueue(curr.Transfer(string.Empty, bossDamage));
            }
        }

        return min.manaSpent.ToString();
    }

    private record State(int manaSpent, int bossHp, int hp, int mana, Dictionary<string, int> effects, bool playersTurn, List<string> spells)
    {
        public State Transfer(string spell, int bossDamage)
        {
            var cost = spell == string.Empty ? 0 : spellList[spell];
            var newManaSpent = manaSpent + cost;
            var newBossHp = bossHp;
            var newHp = hp;
            var newMana = mana - cost;
            var newSpells = new List<string>(spells);
            var hasShield = false;
            if (spell != string.Empty)
            {
                newSpells.Add(spell);
            }
            var newEffects = new Dictionary<string, int>();

            foreach (var effect in effects)
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

            if (!playersTurn)
            {
                newHp -= hasShield ? Math.Max(1, bossDamage - 7) : bossDamage;
                return new State(manaSpent: manaSpent, bossHp: newBossHp, hp: newHp, mana: newMana, effects: newEffects, playersTurn: true, spells: newSpells);
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

            return new State(manaSpent: newManaSpent, bossHp: newBossHp, hp: newHp, mana: newMana, effects: newEffects, playersTurn: false, spells: newSpells);
        }
    }
}