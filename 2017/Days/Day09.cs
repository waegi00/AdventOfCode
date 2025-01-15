using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        var res = 0;
        var isGarbage = false;
        var isIgnored = false;
        var depth = 0;

        foreach (var c in input)
        {
            if (isIgnored)
            {
                isIgnored = false;
                continue;
            }

            if (isGarbage)
            {
                switch (c)
                {
                    case '>':
                        isGarbage = false;
                        break;
                    case '!':
                        isIgnored = true;
                        break;
                }

                continue;
            }

            switch (c)
            {
                case '!':
                    isIgnored = true;
                    break;
                case '<':
                    isGarbage = true;
                    break;
                case '{':
                    depth++;
                    break;
                case '}':
                    res += depth;
                    depth--;
                    break;
            }
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        var res = 0;
        var isGarbage = false;
        var isIgnored = false;

        foreach (var c in input)
        {
            if (isIgnored)
            {
                isIgnored = false;
                continue;
            }

            if (isGarbage)
            {
                switch (c)
                {
                    case '>':
                        isGarbage = false;
                        break;
                    case '!':
                        isIgnored = true;
                        break;
                }

                if (isGarbage && !isIgnored)
                {
                    res++;
                }

                continue;
            }

            switch (c)
            {
                case '!':
                    isIgnored = true;
                    break;
                case '<':
                    isGarbage = true;
                    break;
            }
        }

        return res.ToString();
    }
}