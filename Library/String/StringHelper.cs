namespace AdventOfCode.Library.String;

public static class StringHelper
{
    public static char[][] ToCharArray(this string[] array)
    {
        return array.Select(x => x.ToCharArray()).ToArray();
    }
}