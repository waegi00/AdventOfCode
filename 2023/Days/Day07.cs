using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day7.txt");

        var pokerHands = input.Select(line => line.Split(' '))
            .Select(splits =>
                new PokerHand
                {
                    Hand = splits[0].Trim(),
                    Bid = long.Parse(splits[1].Trim())
                }).ToList();

        var vals = pokerHands.Select(x => new { bid = x.Bid, nums = x.CardValue().ToList() });
        vals = vals
            .OrderBy(x => x.nums[0])
            .ThenBy(x => x.nums[1])
            .ThenBy(x => x.nums[2])
            .ThenBy(x => x.nums[3])
            .ThenBy(x => x.nums[4])
            .ThenBy(x => x.nums[5]);

        return vals.Select((x, i) => x.bid * (i + 1)).Sum().ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day7.txt");

        var pokerHands = input
            .Select(line => line.Split(' '))
            .Select(splits =>
                new PokerHand(true)
                {
                    Hand = splits[0].Trim(),
                    Bid = long.Parse(splits[1].Trim())
                }).ToList();

        var vals = pokerHands.Select(x => new { bid = x.Bid, nums = x.CardValue().ToList() });
        vals = vals
            .OrderBy(x => x.nums[0])
            .ThenBy(x => x.nums[1])
            .ThenBy(x => x.nums[2])
            .ThenBy(x => x.nums[3])
            .ThenBy(x => x.nums[4])
            .ThenBy(x => x.nums[5]);

        return vals.Select((x, i) => x.bid * (i + 1)).Sum().ToString();
    }

    private class PokerHand(bool jackIsWeakest = false)
    {
        private readonly string _hand = string.Empty;
        public string Hand
        {
            get => _hand;
            init
            {
                _hand = value;
                _cards = new();
                foreach (var c in _hand.Where(c => !_cards.TryAdd(c, 1)))
                {
                    _cards[c]++;
                }
            }
        }

        public long Bid { get; init; } = 0;

        private readonly Dictionary<char, long> _cards = new();

        private long JCount => Hand.Count(x => x == 'J');

        private bool IsFiveOfAKind => _cards.Any(x => x.Value == 5)
            || (jackIsWeakest && _cards.Any(x => x.Value + JCount == 5));

        private bool IsFourOfAKind => _cards.Any(x => x.Value == 4)
            || (jackIsWeakest && _cards.Any(x => x.Key != 'J' && x.Value + JCount == 4));

        private bool IsFullHouse => _cards.Count == 2 && _cards.Any(x => x.Value == 3) && _cards.Any(x => x.Value == 2)
            || (jackIsWeakest && _cards.Count == 3 && JCount > 0);

        private bool IsThreeOfAKind => _cards.Any(x => x.Value == 3)
            || (jackIsWeakest && _cards.Any(x => x.Value + JCount == 3));

        private bool IsTwoPair => _cards.Count == 3 && _cards.Count(x => x.Value == 2) == 2
            || (jackIsWeakest && (JCount >= 2 || (_cards.Any(x => x.Value == 2) && JCount == 1)));

        private bool IsOnePair => _cards.Any(x => x.Value == 2)
            || (jackIsWeakest && _cards.Any(x => x.Value + JCount == 2));

        private static bool IsHighCard => true;

        public IEnumerable<long> CardValue()
        {
            yield return Value();

            foreach (var h in Hand)
            {
                yield return CardValue(h);
            }
        }

        private long Value()
        {
            if (IsFiveOfAKind) { return 7; }
            if (IsFourOfAKind) { return 6; }
            if (IsFullHouse) { return 5; }
            if (IsThreeOfAKind) { return 4; }
            if (IsTwoPair) { return 3; }
            if (IsOnePair) { return 2; }
            return IsHighCard ? 1 : 0;
        }

        private long CardValue(char c)
        {
            return c switch
            {
                '2' => 1,
                '3' => 2,
                '4' => 3,
                '5' => 4,
                '6' => 5,
                '7' => 6,
                '8' => 7,
                '9' => 8,
                'T' => 9,
                'J' => jackIsWeakest ? 0 : 10,
                'Q' => 11,
                'K' => 12,
                'A' => 13,
                _ => throw new NotImplementedException(),
            };
        }
    }
}