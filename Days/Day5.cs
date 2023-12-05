using System.Net.Http.Headers;
using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day5 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day5.txt");

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
        //var input = File.ReadAllLines("Days\\Inputs\\Day5.txt");

        //var seeds = new List<long>();

        //var initNums = input[0].Split(":")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();
        //for (var k = 0; k < initNums.Count; k += 2)
        //{
        //    var kk = initNums[k];
        //    for (var n = 0; n < initNums[k + 1]; n++)
        //    {
        //        seeds.Add(kk + n);
        //    }
        //}

        //var maps = new List<Map>();

        //var m = -1;
        //var i = 2;
        //while (i < input.Length)
        //{
        //    var line = input[i];
        //    if (line.EndsWith("map:"))
        //    {
        //        maps.Add(new Map());
        //        m++;
        //    }
        //    else if (line == "")
        //    {
        //        // Nothing happens
        //    }
        //    else
        //    {
        //        var splits = line.Split(' ');
        //        maps[m].Plans.Add(new Plan
        //        {
        //            DestinationRangeStart = long.Parse(splits[0]),
        //            SourceRangeStart = long.Parse(splits[1]),
        //            RangeLength = long.Parse(splits[2]),
        //        });
        //    }
        //    i++;
        //}

        //var minSeed = seeds.Select(seed => maps.Aggregate(seed, (current, map) => map.GetValue(current))).Min();

        //return minSeed.ToString();
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
