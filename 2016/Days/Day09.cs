using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();
        return DecompressedLength(input).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();
        return DecompressedLength(input, true).ToString();
    }

    private static long DecompressedLength(string str, bool doubleDec = false)
    {
        var length = 0L;
        for (var i = 0; i < str.Length;)
        {
            if (str[i] == '(')
            {
                var i2 = i;
                while (str[++i2] != ')') { }

                var split = str[(i + 1)..i2].Split('x').Select(int.Parse).ToList();

                var decompressed = str[(i2 + 1)..(i2 + 1 + split[0])];
                length += (doubleDec ? DecompressedLength(decompressed, doubleDec) : decompressed.Length) * split[1];

                i = i2 + split[0] + 1;
            }
            else
            {
                length++;
                i++;
            }
        }

        return length;
    }
}