using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day10 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var botChips = new Dictionary<int, List<int>>();
        Dictionary<int, (string lowType, int lowId, string highType, int highId)> botActions = new();
        Queue<(int value, int bot)> initialChips = new();

        foreach (var instruction in input)
        {
            var splits = instruction.Split(' ');
            switch (splits[0])
            {
                case "value":
                    initialChips.Enqueue((int.Parse(splits[1]), int.Parse(splits[5])));
                    break;
                case "bot":
                    botActions[int.Parse(splits[1])] = (splits[5], int.Parse(splits[6]), splits[10], int.Parse(splits[11]));
                    break;
            }
        }

        while (initialChips.Count > 0)
        {
            var (value, bot) = initialChips.Dequeue();

            if (!botChips.TryAdd(bot, [value]))
            {
                botChips[bot].Add(value);
            }

            if (botChips[bot].Count != 2) continue;

            var chips = botChips[bot].OrderBy(x => x).ToList();
            var lowChip = chips[0];
            var highChip = chips[1];

            if (lowChip == 17 && highChip == 61)
            {
                return bot.ToString();
            }

            var (lowType, lowId, highType, highId) = botActions[bot];
            if (lowType == "bot")
            {
                initialChips.Enqueue((lowChip, lowId));
            }
            if (highType == "bot")
            {
                initialChips.Enqueue((highChip, highId));
            }

            botChips[bot].Clear();
        }

        return "";
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var botChips = new Dictionary<int, List<int>>();
        var outChips = new Dictionary<int, int>();
        Dictionary<int, (string lowType, int lowId, string highType, int highId)> botActions = new();
        Queue<(int value, int bot)> initialChips = new();

        foreach (var instruction in input)
        {
            var splits = instruction.Split(' ');
            switch (splits[0])
            {
                case "value":
                    initialChips.Enqueue((int.Parse(splits[1]), int.Parse(splits[5])));
                    break;
                case "bot":
                    botActions[int.Parse(splits[1])] = (splits[5], int.Parse(splits[6]), splits[10], int.Parse(splits[11]));
                    break;
            }
        }

        while (initialChips.Count > 0)
        {
            var (value, bot) = initialChips.Dequeue();

            if (!botChips.TryAdd(bot, [value]))
            {
                botChips[bot].Add(value);
            }

            if (botChips[bot].Count != 2) continue;

            var chips = botChips[bot].OrderBy(x => x).ToList();
            var lowChip = chips[0];
            var highChip = chips[1];

            var (lowType, lowId, highType, highId) = botActions[bot];
            if (lowType == "bot")
            {
                initialChips.Enqueue((lowChip, lowId));
            }
            else
            {
                outChips.Add(lowId, lowChip);
            }
            if (highType == "bot")
            {
                initialChips.Enqueue((highChip, highId));
            }
            else
            {
                outChips.Add(highId, highChip);
            }

            botChips[bot].Clear();
        }

        return (outChips[0] * outChips[1] * outChips[2]).ToString();
    }
}
