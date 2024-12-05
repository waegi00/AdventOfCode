using AdventOfCode.Interfaces;

namespace AdventOfCode.Library.Input;

public static class InputParser
{
    public static string InputToText(this IRiddle riddle, bool test = false)
    {
        return File.ReadAllText(Path(riddle, test));
    }

    public static string[] InputToLines(this IRiddle riddle, bool test = false)
    {
        return File.ReadAllLines(Path(riddle, test));
    }

    private static string Path(IRiddle riddle, bool test)
    {
        var path = $@"{riddle.GetType().ToString().Split("._")[1][..4]}\Days\Inputs\{riddle.GetType().Name}";

        if (test)
        {
            path += "test";
        }

        return $"{path}.txt";
    }
}