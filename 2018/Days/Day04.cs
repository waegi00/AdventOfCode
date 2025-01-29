using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        var sleepData = GetSleepData();

        var maxSleepGuard = sleepData
            .OrderByDescending(x => x.Value.Sum())
            .First()
            .Key;

        var maxSleepMinute = sleepData[maxSleepGuard]
            .Select((count, minute) => new { minute, count })
            .OrderByDescending(x => x.count)
            .First()
            .minute;

        return (maxSleepGuard * maxSleepMinute).ToString();
    }

    public string SolveSecond()
    {
        var sleepData = GetSleepData();

        var mostFrequentSleep = sleepData
            .SelectMany(guard => guard.Value
                .Select((count, minute) => new
                {
                    GuardId = guard.Key,
                    Minute = minute,
                    SleepCount = count
                }))
            .OrderByDescending(x => x.SleepCount)
            .First();

        return (mostFrequentSleep.GuardId * mostFrequentSleep.Minute).ToString();
    }

    private Dictionary<int, int[]> GetSleepData()
    {
        var logs = this.InputToLines()
            .Select(x => x.Split("] "))
            .Select(x => (Date: DateTime.Parse(x[0][1..]), Action: x[1]))
            .OrderBy(x => x.Date)
            .ToArray();

        var sleepData = new Dictionary<int, int[]>();
        var currentGuardId = 0;
        var sleepStartTime = DateTime.MinValue;

        foreach (var (date, action) in logs)
        {
            if (action.StartsWith("Guard"))
            {
                currentGuardId = int.Parse(action.Split(' ')[1][1..]);

                if (!sleepData.ContainsKey(currentGuardId))
                {
                    sleepData[currentGuardId] = new int[60];
                }
            }
            else
                switch (action)
                {
                    case "falls asleep":
                        sleepStartTime = date;
                        break;
                    case "wakes up":
                        {
                            for (var i = sleepStartTime.Minute; i < date.Minute; i++)
                            {
                                sleepData[currentGuardId][i]++;
                            }

                            break;
                        }
                }
        }

        return sleepData;
    }
}