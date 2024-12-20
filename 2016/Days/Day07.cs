using AdventOfCode.Interfaces;
using AdventOfCode.Library.Char;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        return this.InputToLines().Count(HasAbba).ToString();
    }

    public string SolveSecond()
    {
        return this.InputToLines().Count(HasAbaAndBab).ToString();
    }

    private static bool HasAbba(string str)
    {
        var res = false;

        var brackets = 0;
        for (var i = 0; i < str.Length - 3; i++)
        {
            switch (str[i])
            {
                case '[':
                    brackets++;
                    break;
                case ']':
                    brackets--;
                    break;
                default:
                    if (str[i] == str[i + 3] && str[i + 1] == str[i + 2] && str[i] != str[i + 1])
                    {
                        if (brackets > 0)
                        {
                            return false;
                        }

                        res = true;
                    }

                    break;
            }
        }

        return res;
    }

    private static bool HasAbaAndBab(string str)
    {
        var inside = new HashSet<(char, char)>();
        var outside = new HashSet<(char, char)>();

        var brackets = 0;
        for (var i = 0; i < str.Length - 2; i++)
        {
            switch (str[i])
            {
                case '[':
                    brackets++;
                    break;
                case ']':
                    brackets--;
                    break;
                default:
                    if (str[i] == str[i + 2] && str[i + 1] != str[i] && str[i].IsLower() && str[i + 1].IsLower())
                    {
                        if (brackets > 0)
                        {
                            inside.Add((str[i], str[i + 1]));
                        }
                        else
                        {
                            outside.Add((str[i + 1], str[i]));
                        }
                    }

                    break;
            }
        }

        return inside.Any(outside.Contains);
    }
}