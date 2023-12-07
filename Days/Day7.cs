using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day7 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day7.txt");

        var pokerHands = new List<PokerHand>();

        foreach (var line in input)
        {
            var splits = line.Split(' ');
            pokerHands.Add(new PokerHand { Hand = splits[0].Trim(), Bid = long.Parse(splits[1].Trim()) });
        }

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
        var input = File.ReadAllLines("Days\\Inputs\\Day7.txt");

        var pokerHands = new List<PokerHand>();

        foreach (var line in input)
        {
            var splits = line.Split(' ');
            pokerHands.Add(new PokerHand(true) { Hand = splits[0].Trim(), Bid = long.Parse(splits[1].Trim()) });
        }

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
        private bool jIsWeakest = jackIsWeakest;
        private string _hand = string.Empty;
        public string Hand
        {
            get { return _hand; }
            set
            {
                _hand = value;
                cards = new Dictionary<char, long>();
                foreach (var c in _hand)
                {
                    if (!cards.TryAdd(c, 1))
                    {
                        cards[c]++;
                    }
                }
            }
        }

        public long Bid { get; set; } = 0;

        private Dictionary<char, long> cards = new Dictionary<char, long>();

        private long jCount => Hand.Count(x => x == 'J');

        private bool IsFiveOfAKind => cards.Any(x => x.Value == 5)
            || (jIsWeakest && cards.Any(x => x.Value + jCount == 5));

        private bool IsFourOfAKind => cards.Any(x => x.Value == 4)
            || (jIsWeakest && cards.Any(x => x.Key != 'J' && x.Value + jCount == 4));

        private bool IsFullHouse => cards.Count == 2 && cards.Any(x => x.Value == 3) && cards.Any(x => x.Value == 2)
            || (jIsWeakest && cards.Count == 3 && jCount > 0);

        private bool IsThreeOfAKind => cards.Any(x => x.Value == 3)
            || (jIsWeakest && cards.Any(x => x.Value + jCount == 3));

        private bool IsTwoPair => cards.Count == 3 && cards.Where(x => x.Value == 2).Count() == 2
            || (jIsWeakest && (jCount >= 2 || (cards.Any(x => x.Value == 2) && jCount == 1)));

        private bool IsOnePair => cards.Any(x => x.Value == 2)
            || (jIsWeakest && cards.Any(x => x.Value + jCount == 2));

        private bool IsHighCard => true;

        public IEnumerable<long> CardValue()
        {
            yield return Value();

            for (var i = 0; i < Hand.Length; i++)
            {
                yield return CardValue(Hand[i]);
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
            if (IsHighCard) { return 1; }
            return 0;
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
                'J' => jIsWeakest ? 0 : 10,
                'Q' => 11,
                'K' => 12,
                'A' => 13,
                _ => throw new NotImplementedException(),
            };
        }
    }
}