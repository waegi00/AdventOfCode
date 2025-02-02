﻿using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day01 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day1.txt");

        var sum = input.Sum(FirstAndLastDigit);

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day1.txt");

        var sum = input.Sum(FirstAndLastDigitWithText);

        return sum.ToString();
    }

    private static int FirstAndLastDigit(string item)
    {
        int first = 0, last = 0;

        foreach (var elem in item.Where(char.IsNumber))
        {
            first = int.Parse(elem.ToString());
            break;
        }

        for (var i = item.Length - 1; i >= 0; i--)
        {
            if (!char.IsNumber(item[i])) continue;
            last = int.Parse(item[i].ToString());
            break;
        }

        return first * 10 + last;
    }

    private static int FirstAndLastDigitWithText(string item)
    {
        var texts = new List<string>
        {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
            "ten"
        };

        int first = 0, last = 0;

        for (var i = 0; i < item.Length; i++)
        {
            if (char.IsNumber(item[i]))
            {
                first = int.Parse(item[i].ToString());
                break;
            }

            foreach (var text in texts.Where(text => i + text.Length <= item.Length && item.Substring(i, text.Length) == text))
            {
                first = texts.IndexOf(text) + 1;
                goto A;
            }
        }
        A:

        for (var i = item.Length - 1; i >= 0; i--)
        {
            if (char.IsNumber(item[i]))
            {
                last = int.Parse(item[i].ToString());
                break;
            }

            foreach (var text in texts.Where(text => i + text.Length <= item.Length && item.Substring(i, text.Length) == text))
            {
                last = texts.IndexOf(text) + 1;
                goto B;
            }
        }
        B:

        return first * 10 + last;
    }
}
