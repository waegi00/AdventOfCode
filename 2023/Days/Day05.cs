using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day5.txt");

        var seeds = input[0].Split(":")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();
        var maps = new List<Map>();

        var m = -1;
        var i = 2;
        while (i < input.Length)
        {
            var line = input[i];
            if (line.EndsWith("map:"))
            {
                maps.Add(new Map());
                m++;
            }
            else if (line == "")
            {
                // Nothing happens
            }
            else
            {
                var splits = line.Split(' ');
                maps[m].Plans.Add(new Plan
                {
                    DestinationRangeStart = long.Parse(splits[0]),
                    SourceRangeStart = long.Parse(splits[1]),
                    RangeLength = long.Parse(splits[2]),
                });
            }
            i++;
        }

        var minSeed = seeds.Select(seed => maps.Aggregate(seed, (current, map) => map.GetValue(current))).Min();

        return minSeed.ToString();
    }

    public string SolveSecond()
    {
        return "";
    }

    private class Map
    {
        public List<Plan> Plans { get; set; } = new List<Plan>();

        public long GetValue(long source)
        {
            foreach (var plan in Plans)
            {
                try
                {
                    return plan.GetValue(source);
                }
                catch
                {
                    // Continue
                }

            }
            return source;
        }
    }

    private class Plan
    {
        public long DestinationRangeStart { get; init; }
        public long SourceRangeStart { get; init; }
        public long RangeLength { get; init; }

        public long GetValue(long source)
        {
            if (source < SourceRangeStart || source >= SourceRangeStart + RangeLength)
            {
                throw new ArgumentException(nameof(source));
            }

            return DestinationRangeStart - SourceRangeStart + source;
        }
    }
}
