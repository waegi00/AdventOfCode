using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        var (numbers, cards) = ParseInput();

        foreach (var num in numbers)
        {
            foreach (var card in cards)
            {
                if (card.TryAdd(num, out var result))
                {
                    return result.ToString();
                }
            }
        }

        return "";
    }

    public string SolveSecond()
    {
        var (numbers, cards) = ParseInput();

        foreach (var num in numbers)
        {
            foreach (var card in cards.ToArray())
            {
                if (!card.TryAdd(num, out var result)) continue;
                if (cards.Count == 1)
                {
                    return result.ToString();
                }

                cards.Remove(card);
            }
        }

        return "";
    }

    private (List<int>, List<BingoCard>) ParseInput()
    {
        var input = this.InputToText()
            .Split("\r\n\r\n")
            .ToArray();

        var numbers = input[0]
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var cards = input[1..]
            .Select(x => new BingoCard(x.Split("\r\n").Select(y =>
                    y.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray())
                .ToArray()))
            .ToList();

        return (numbers, cards);
    }

    private class BingoCard
    {
        private readonly int[][] Card;
        private readonly bool[][] Seen;

        public BingoCard(int[][] card)
        {
            Card = card;
            Seen = card.Select(x => x.Select(_ => false).ToArray()).ToArray();
        }

        public bool TryAdd(int num, out int result)
        {
            result = -1;

            for (var i = 0; i < Card.Length; i++)
            {
                for (var j = 0; j < Card[i].Length; j++)
                {
                    if (Card[i][j] != num) continue;

                    Seen[i][j] = true;
                    if (!Seen[i].All(x => x) && !Seen.Select(x => x[j]).All(x => x)) continue;
                    result = Score(num);
                    return true;
                }
            }

            return false;
        }

        private int Score(int num)
        {
            return num * Card.WithIndex()
                .Sum(x => x.item
                    .WithIndex()
                    .Select(y => (x, y))
                    .Where(pair => !Seen[pair.x.index][pair.y.index])
                    .Sum(pair => pair.y.item));
        }
    }
}