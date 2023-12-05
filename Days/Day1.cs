using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day1 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day1.txt");

        var sum = 0;

        foreach (var item in input)
        {
            sum += FirstAndLastDigit(item);
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day1.txt");

        var sum = 0;

        foreach (var item in input)
        {
            sum += FirstAndLastDigitWithText(item);
        }

        return sum.ToString();
    }

    private int FirstAndLastDigit(string item)
    {
        int first = 0, last = 0;

        for (var i = 0; i < item.Length; i++)
        {
            if (char.IsNumber(item[i]))
            {
                first = int.Parse(item[i].ToString());
                break;
            }
        }

        for (var i = item.Length - 1; i >= 0; i--)
        {
            if (char.IsNumber(item[i]))
            {
                last = int.Parse(item[i].ToString());
                break;
            }
        }

        return first * 10 + last;
    }

    private int FirstAndLastDigitWithText(string item)
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

            foreach (var text in texts)
            {
                if (i + text.Length <= item.Length && item.Substring(i, text.Length) == text)
                {
                    first = texts.IndexOf(text) + 1;
                    goto A;
                }
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

            foreach (var text in texts)
            {
                if (i + text.Length <= item.Length && item.Substring(i, text.Length) == text)
                {
                    last = texts.IndexOf(text) + 1;
                    goto B;
                }
            }
        }
        B:

        return first * 10 + last;
    }
}
