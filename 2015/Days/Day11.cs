using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;
namespace AdventOfCode._2015.Days;

public class Day11 : IRiddle
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

    private static bool ValidatePassword(string password)
    {
        if (password.Contains('i') || password.Contains('l') || password.Contains('o')) return false;
        if (!Regex.IsMatch(password, @".*(.)\1+.*(.)\2+.*")) return false;

        for (var i = 0; i < password.Length - 2; i++)
        {
            if (password[i + 2] == password[i + 1] + 1 && password[i + 1] == password[i] + 1)
            {
                return true;
            }
        }

        return false;
    }
}