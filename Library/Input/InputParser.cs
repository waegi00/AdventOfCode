using AdventOfCode.Interfaces;

namespace AdventOfCode.Library.Input;

public static class InputParser
{
    public static string InputToText(this IRiddle riddle)
    {
        return File.ReadAllText(Path(riddle));
    }

    public static string[] InputToLines(this IRiddle riddle)
    {
        return File.ReadAllLines(Path(riddle));
    }

    private static string Path(IRiddle riddle)
    {
        return $@"{riddle.GetType().ToString().Split("._")[1][..4]}\Days\Inputs\{riddle.GetType().Name}.txt";
    }
}