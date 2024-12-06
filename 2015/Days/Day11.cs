using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;
namespace AdventOfCode._2015.Days;

internal class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        do
        {
            input = input.Next();
        } while (!ValidatePassword(input));

        return input;
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        do
        {
            input = input.Next();
        } while (!ValidatePassword(input));

        do
        {
            input = input.Next();
        } while (!ValidatePassword(input));

        return input;
    }

    private static bool ValidatePassword(string Password)
    {
        if (Password.Contains('i') || Password.Contains('l') || Password.Contains('o')) return false;
        if (!Regex.IsMatch(Password, @".*(.)\1+.*(.)\2+.*")) return false;

        for (var i = 0; i < Password.Length - 2; i++)
        {
            if (Password[i + 2] == Password[i + 1] + 1 && Password[i + 1] == Password[i] + 1)
            {
                return true;
            }
        }

        return false;
    }
}