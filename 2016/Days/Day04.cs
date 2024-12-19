using AdventOfCode.Interfaces;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day04 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var sum = 0;

        foreach (var line in input)
        {
            var splits = line.Split("[");
            var checksum = splits[1][..^1];

            var room = splits[0].Split('-');

            var letters = room[..^1].SelectMany(l => l);
            var frequencies = letters
                .GroupBy(c => c)
                .ToDictionary(c => c.Key, c => c.Count());

            var a = new string(frequencies
                .OrderByDescending(f => f.Value)
                .ThenBy(f => f.Key)
                .Select(f => f.Key)
                .Take(5)
                .ToArray());
            if (checksum == a)
            {
                sum += int.Parse(room[^1]);
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        foreach (var line in input)
        {
            var splits = line.Split("[");

            var room = splits[0].Replace('-', ' ');
            var id = int.Parse(room.Split(' ')[^1]);

            if (RotN(room, id).StartsWith("northpole object storage"))
            {
                return id.ToString();
            }
        }

        return "";
    }

    private static string RotN(string str, int n)
    {
        return new string(str.Select(c => c.IsLower() ? (char)((c - 'a' + n) % 26 + 'a') : c).ToArray());
    }
}
