using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Text.RegularExpressions;

namespace AdventOfCode._2022.Days;

public partial class Day19 : IRiddle
{
    private int[]? maxRobots;
    private int maxGeodes;

    public string SolveFirst()
    {
        return Solve();
    }

    public string SolveSecond()
    {
        return Solve(true);
    }

    private string Solve(bool isPart2 = false)
    {
        return FindBlueprintScores(this.InputToLines().Select(Parse).ToArray(), isPart2)
            .ToString();
    }

    private static int[][] Parse(string line)
    {
        var numbers = NumberRegex().Matches(line)
            .Select(m => int.Parse(m.Value))
            .ToArray();

        return new int[][]
        {
            [numbers[1], 0, 0],
            [numbers[2], 0, 0],
            [numbers[3], numbers[4], 0],
            [numbers[5], 0, numbers[6]],
        };
    }

    private int FindBlueprintScores(int[][][] blueprints, bool isPart2 = false)
    {
        var input = isPart2 ? 1 : 0;

        for (var i = 0; i < (isPart2 ? 3 : blueprints.Length); i++)
        {
            FindMaxRobots(blueprints[i]);
            FindMaxGeodes(blueprints[i], new int[4], [1, 0, 0, 0], isPart2 ? 32 : 24);

            if (!isPart2)
            {
                input += (i + 1) * maxGeodes;
            }
            else
            {
                input *= maxGeodes;
            }

            maxGeodes = 0;
        }

        return input;
    }

    private void FindMaxRobots(int[][] blueprint)
    {
        maxRobots = new int[3];
        for (var j = 0; j < 3; j++)
        {
            for (var i = 0; i < 4; i++)
            {
                maxRobots[j] = Math.Max(maxRobots[j], blueprint[i][j]);
            }
        }
    }

    private int FindMaxGeodes(int[][] blueprint, int[] resources, int[] robots, int time)
    {
        var plannedGeodes = resources[3] + robots[3] * time;
        if (plannedGeodes + (time * time - time) / 2 <= maxGeodes) return 0;

        for (var i = 0; i < 4; i++)
        {
            if (i < 3 && maxRobots![i] <= robots[i]) continue;
            var wait = TimeForResources(blueprint[i], resources, robots) + 1;
            if (time - wait < 1) continue;

            var resourceCopy = (int[])resources.Clone();
            var robotCopy = (int[])robots.Clone();

            for (var j = 0; j < 4; j++) resourceCopy[j] += robots[j] * wait;
            for (var j = 0; j < 3; j++) resourceCopy[j] -= blueprint[i][j];
            robotCopy[i]++;

            plannedGeodes = Math.Max(plannedGeodes, FindMaxGeodes(blueprint, resourceCopy, robotCopy, time - wait));
        }

        maxGeodes = Math.Max(maxGeodes, plannedGeodes);
        return plannedGeodes;
    }

    private static int TimeForResources(int[] cost, int[] resources, int[] robots)
    {
        var maxTime = 0;
        for (var i = 0; i < 3; i++)
        {
            if (cost[i] == 0) continue;
            if (robots[i] == 0) return 1000;
            maxTime = Math.Max(maxTime, (cost[i] - resources[i] - 1 + robots[i]) / robots[i]);
        }
        return maxTime;
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();
}