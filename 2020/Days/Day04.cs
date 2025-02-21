using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public partial class Day04 : IRiddle
{

    private static readonly HashSet<string> ValidEyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

    private static readonly string[] RequiredFields = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];

    public string SolveFirst()
    {
        var passports = this.InputToText()
            .Split("\r\n\r\n")
            .Select(raw => raw.Replace("\r\n", " ")
                .Split(' ')
                .Select(field => field.Split(':'))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0], parts => parts[1]))
            .Where(passport => RequiredFields.All(passport.ContainsKey))
            .ToList();

        return passports.Count.ToString();
    }

    public string SolveSecond()
    {
        var passports = this.InputToText()
            .Split("\r\n\r\n")
            .Select(raw => raw.Replace("\r\n", " ")
                .Split(' ')
                .Select(field => field.Split(':'))
                .Where(parts => parts.Length == 2)
                .ToDictionary(parts => parts[0], parts => parts[1]))
            .Where(passport => RequiredFields.All(passport.ContainsKey))
            .ToList();

        return passports.Count(IsValidPassport).ToString();
    }

    private static bool IsValidPassport(Dictionary<string, string> passport)
    {
        if (!int.TryParse(passport["byr"], out var byr) || byr is < 1920 or > 2002) return false;
        if (!int.TryParse(passport["iyr"], out var iyr) || iyr is < 2010 or > 2020) return false;
        if (!int.TryParse(passport["eyr"], out var eyr) || eyr is < 2020 or > 2030) return false;

        if (!HeightRegex().IsMatch(passport["hgt"])) return false;

        var heightMatch = HeightRegex().Match(passport["hgt"]);
        var height = int.Parse(heightMatch.Groups[1].Value);

        if (heightMatch.Groups[2].Value == "cm" && height is < 150 or > 193) return false;
        if (heightMatch.Groups[2].Value == "in" && height is < 59 or > 76) return false;

        if (!HairColorRegex().IsMatch(passport["hcl"])) return false;

        return ValidEyeColors.Contains(passport["ecl"]) && PassportIdRegex().IsMatch(passport["pid"]);
    }

    [GeneratedRegex(@"^(\d+)(cm|in)$")]
    private static partial Regex HeightRegex();

    [GeneratedRegex("^#[0-9a-f]{6}$")]
    private static partial Regex HairColorRegex();

    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex PassportIdRegex();
}