using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var vowelPattern = new Regex(@"[aeiou]");
        var doubleLetterPattern = new Regex(@"(.)\1");
        var forbiddenPattern = new Regex(@"(ab|cd|pq|xy)");

        var sum = input.Count(line =>
            vowelPattern.Matches(line).Count > 2 &&
            doubleLetterPattern.IsMatch(line) &&
            !forbiddenPattern.IsMatch(line));

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var pairPattern = new Regex(@"(?=(..).*\1)");
        var repeatPattern = new Regex(@"(.).\1");

        var sum = input.Count(line =>
            pairPattern.IsMatch(line) &&
            repeatPattern.IsMatch(line));

        return sum.ToString();
    }
}