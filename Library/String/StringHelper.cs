using AdventOfCode.Library.Char;

namespace AdventOfCode.Library.String;

public static class StringHelper
{
    public static char[][] ToCharArray(this string[] array)
    {
        return array.Select(x => x.ToCharArray()).ToArray();
    }

    public static string Next(this string str)
    {
        return str.Next(str.Length - 1);
    }

    public static string Next(this string str, int index)
    {
        var chars = str.ToCharArray();

        if (chars.Any(c => !c.IsLower() || !c.IsLower())) return str;
        if (index < 0 || index >= chars.Length) return str;

        while (index >= 0)
        {
            if (chars[index].IsLower() && chars[index] == 'z')
            {
                chars[index] = 'a';
                index--;
            }
            else if (chars[index].IsUpper())
            {
                chars[index] = 'A';
                index--;
            }
            else
            {
                chars[index]++;
                return new string(chars);
            }
        }

        return new string('a', chars.Length + 1);
    }
}