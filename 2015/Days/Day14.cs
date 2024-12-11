using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var regex = new Regex(@"\d+");

        var reindeers = new List<(int speed, int time, int pause)>();

        foreach (var line in input)
        {
            var nums = new List<int>(3);
            foreach (Match match in regex.Matches(line))
            {
                nums.Add(int.Parse(match.Value));
            }

            reindeers.Add((nums[0], nums[1], nums[2]));
        }

        var maxDist = 0;

        foreach (var reindeer in reindeers)
        {
            var dist = 0;
            var duration = 2503;
            while (duration >= 0)
            {
                dist += Math.Min(duration, reindeer.time) * reindeer.speed;
                duration -= reindeer.time;
                duration -= reindeer.pause;
            }

            maxDist = Math.Max(maxDist, dist);
        }

        return maxDist.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var regex = new Regex(@"\d+");

        var reindeers = new List<(int speed, int time, int pause)>();

        foreach (var line in input)
        {
            var nums = new List<int>(3);
            foreach (Match match in regex.Matches(line))
            {
                nums.Add(int.Parse(match.Value));
            }

            reindeers.Add((nums[0], nums[1], nums[2]));
        }

        var stats = new List<(int points, int dist, int timeLeft, int pauseLeft)>();

        for (var i = 0; i < reindeers.Count; i++)
        {
            stats.Add((0, 0, reindeers[i].time, reindeers[i].pause));
        }

        for (var i = 0; i < 2503; i++)
        {
            for (var r = 0; r < reindeers.Count; r++)
            {
                if (stats[r].timeLeft > 0)
                {
                    stats[r] = (stats[r].points, stats[r].dist + reindeers[r].speed, stats[r].timeLeft - 1, stats[r].pauseLeft);
                }
                else if (stats[r].pauseLeft > 0)
                {
                    stats[r] = (stats[r].points, stats[r].dist, 0, stats[r].pauseLeft - 1);
                }
                else
                {
                    stats[r] = (stats[r].points, stats[r].dist + reindeers[r].speed, reindeers[r].time - 1, reindeers[r].pause);
                }
            }

            var indices = stats
                .Select((s, index) => (s, index))
                .Where(s => s.s.dist == stats.Max(stat => stat.dist))
                .Select(s => s.index)
                .ToList();

            foreach (var index in indices)
            {
                stats[index] = (stats[index].points + 1, stats[index].dist, stats[index].timeLeft, stats[index].pauseLeft);
            }
        }

        return stats.Max(s => s.points).ToString();
    }
}