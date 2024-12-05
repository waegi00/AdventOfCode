using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day06 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day6.txt");

        var dic = new Dictionary<int, int>();

        input[0].Split(':')[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList().ForEach(x => dic.Add(x, 0));
        input[1].Split(':')[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select((x, i) => new { x = int.Parse(x), i }).ToList().ForEach(x => dic[dic.ElementAt(x.i).Key] = x.x);

        var ways = 1;
        foreach (var keyValue in dic)
        {
            var raceWays = 0;
            var key = keyValue.Key;
            var value = keyValue.Value;

            for (var vel = 0; vel <= key; vel++)
            {
                var dist = vel * (key - vel);
                if (dist > value)
                {
                    raceWays++;
                }
            }

            ways *= raceWays;
        }

        return ways.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day6.txt");

        var time = long.Parse(input[0].Split(':')[1].Replace(" ", ""));
        var record = long.Parse(input[1].Split(':')[1].Replace(" ", ""));

        var ways = 0;

        for (var vel = 0; vel <= time; vel++)
        {
            var dist = vel * (time - vel);
            if (dist > record)
            {
                ways++;
            }
        }

        return ways.ToString();
    }
}
