using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016.Days;

public partial class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var salt = this.InputToText();

        var threes = new List<(char key, int pos)>();
        var keys = new List<int>();
        var i = 0;

        while (keys.Count < 64)
        {
            var hashString = Convert.ToHexString(MD5.HashData(Encoding.ASCII.GetBytes(salt + i)));

            var matches = ThreepeatRegex().Matches(hashString);
            if (matches.Count > 0)
            {
                foreach (Match match in FivepeatRegex().Matches(hashString))
                {
                    var c = match.Value[0];
                    foreach (var (k, p) in threes.Where(x => x.pos + 1000 >= i).ToList())
                    {
                        if (k != c) continue;
                        keys.Add(p);
                        threes.Remove((k, p));
                    }
                }

                threes.Add((matches[0].Value[0], i));
            }

            i++;
        }

        return keys.Order().ToArray()[63].ToString();
    }

    public string SolveSecond()
    {
        var salt = this.InputToText();

        var threes = new List<(char key, int pos)>();
        var keys = new List<int>();
        var i = 0;

        while (keys.Count < 64)
        {
            var hashData = MD5.HashData(Encoding.ASCII.GetBytes(salt + i));
            for (var k = 0; k < 2016; k++)
            {
                hashData = MD5.HashData(Encoding.ASCII.GetBytes(Convert.ToHexString(hashData).ToLower()));
            }

            var hashString = Convert.ToHexString(hashData);

            var matches = ThreepeatRegex().Matches(hashString);
            if (matches.Count > 0)
            {
                foreach (Match match in FivepeatRegex().Matches(hashString))
                {
                    var c = match.Value[0];
                    foreach (var (k, p) in threes.Where(x => x.pos + 1000 >= i).ToList())
                    {
                        if (k != c) continue;
                        keys.Add(p);
                        threes.Remove((k, p));
                    }
                }

                threes.Add((matches[0].Value[0], i));
            }

            i++;
        }

        return keys.Order().ToArray()[63].ToString();
    }

    [GeneratedRegex(@"(.)\1\1")]
    private static partial Regex ThreepeatRegex();

    [GeneratedRegex(@"(.)\1\1\1\1")]
    private static partial Regex FivepeatRegex();
}
