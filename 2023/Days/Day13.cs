using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllText("2023\\Days\\Inputs\\Day13.txt");

        var sum = 0;

        foreach (var pattern in input.Split("\r\n\r\n"))
        {
            var lines = pattern.Split("\r\n");
            var found = false;

            for (var row = 1; !found && row < lines.Length; row++)
            {
                var failed = false;
                for (var diff = 1; !failed && !found && row - diff >= 0 && row + diff - 1 < lines.Length; diff++)
                {
                    if (lines[row - diff] != lines[row + diff - 1])
                    {
                        failed = true;
                    }
                }

                if (!failed)
                {
                    sum += 100 * row;
                    found = true;
                }
            }

            if (found) continue;

            var rotatedLines = "";

            for (var i = 0; i < lines[0].Length; i++)
            {
                foreach (var line in lines)
                {
                    rotatedLines += line[i];
                }
                rotatedLines += "\r\n";
            }

            lines = rotatedLines.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


            for (var row = 1; !found && row < lines.Length; row++)
            {
                var failed = false;
                for (var diff = 1; !failed && !found && row - diff >= 0 && row + diff - 1 < lines.Length; diff++)
                {
                    if (lines[row - diff] != lines[row + diff - 1])
                    {
                        failed = true;
                    }
                }

                if (!failed)
                {
                    sum += row;
                    found = true;
                }
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllText("2023\\Days\\Inputs\\Day13.txt");

        var sum = 0;

        foreach (var pattern in input.Split("\r\n\r\n"))
        {
            var lines = pattern.Split("\r\n");
            var found = false;

            for (var row = 1; !found && row < lines.Length; row++)
            {
                var failed = 0;
                for (var diff = 1; !found && row - diff >= 0 && row + diff - 1 < lines.Length; diff++)
                {
                    var first = lines[row - diff];
                    var second = lines[row + diff - 1];
                    failed += first.Where((t, i) => t != second[i]).Count();
                }

                if (failed == 1)
                {
                    sum += 100 * row;
                    found = true;
                }
            }

            if (found) continue;

            var rotatedLines = "";

            for (var i = 0; i < lines[0].Length; i++)
            {
                foreach (var line in lines)
                {
                    rotatedLines += line[i];
                }
                rotatedLines += "\r\n";
            }

            lines = rotatedLines.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


            for (var row = 1; !found && row < lines.Length; row++)
            {
                var failed = 0;
                for (var diff = 1; !found && row - diff >= 0 && row + diff - 1 < lines.Length; diff++)
                {
                    var first = lines[row - diff];
                    var second = lines[row + diff - 1];
                    failed += first.Where((t, i) => t != second[i]).Count();
                }

                if (failed == 1)
                {
                    sum += row;
                    found = true;
                }
            }
        }

        return sum.ToString();
    }
}