using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day21 : IRiddle
{
    private readonly List<(int cost, int damage, int armor)> _weaponList = [(8, 4, 0), (10, 5, 0), (25, 6, 0), (40, 7, 0), (74, 8, 0)];
    private readonly List<(int cost, int damage, int armor)> _armorList = [(13, 0, 1), (31, 0, 2), (53, 0, 3), (75, 0, 4), (102, 0, 5)];
    private readonly List<(int cost, int damage, int armor)> _ringList = [(25, 1, 0), (50, 2, 0), (100, 3, 0), (20, 0, 1), (40, 0, 2), (80, 0, 3)];
    private readonly List<(int cost, int damage, int armor)> _empty = [(0, 0, 0)];

    public string SolveFirst()
    {
        var input = this.InputToLines().Select(x => int.Parse(x.Split(": ")[1])).ToList();
        var boss = (input[0], input[1], input[2]);

        var min = int.MaxValue;
        foreach (var weapon in _weaponList)
        {
            foreach (var armor in _empty.Concat(_armorList))
            {
                foreach (var ring in _empty.Concat(_ringList).Concat(_ringList.Pairs().Select(x => (x.First.cost + x.Second.cost, x.First.damage + x.Second.damage, x.First.armor + x.Second.armor))))
                {
                    var cost = weapon.cost + armor.cost + ring.Item1;
                    if (cost >= min)
                    {
                        continue;
                    }

                    var player = (
                        100,
                        weapon.damage + armor.damage + ring.Item2,
                        weapon.armor + armor.armor + ring.Item3);

                    if (Play(player, boss) == true)
                    {
                        min = Math.Min(min, cost);
                    }
                }
            }
        }

        return min.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().Select(x => int.Parse(x.Split(": ")[1])).ToList();
        var boss = (input[0], input[1], input[2]);

        var max = 0;
        foreach (var weapon in _weaponList)
        {
            foreach (var armor in _empty.Concat(_armorList))
            {
                foreach (var ring in _empty.Concat(_ringList).Concat(_ringList.Pairs().Select(x =>
                             (x.First.cost + x.Second.cost, x.First.damage + x.Second.damage,
                                 x.First.armor + x.Second.armor))))
                {
                    var cost = weapon.cost + armor.cost + ring.Item1;
                    if (cost <= max)
                    {
                        continue;
                    }

                    var player = (
                        100,
                        weapon.damage + armor.damage + ring.Item2,
                        weapon.armor + armor.armor + ring.Item3);

                    if (Play(player, boss) == false)
                    {
                        max = Math.Max(max, cost);
                    }
                }
            }
        }

        return max.ToString();
    }

    private static bool? Play((int health, int damage, int armor) player, (int health, int damage, int armor) boss)
    {
        var playerCanBeatBoss = boss.armor < player.damage;
        var bossCanBeatPlayer = player.armor < boss.damage;

        if (!playerCanBeatBoss && !bossCanBeatPlayer) return null;
        if (!playerCanBeatBoss || !bossCanBeatPlayer) return playerCanBeatBoss;

        return Math.Ceiling((double)player.health / (boss.damage - player.armor)) >= Math.Ceiling((double)boss.health / (player.damage - boss.armor));
    }
}