using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2020.Days;

public class Day25 : IRiddle
{
    public string SolveFirst()
    {
        var publicKeys = this.InputToLines()
            .Select(long.Parse)
            .ToArray();

        const int modulus = 20201227;
        var cardPublicKey = publicKeys[0];
        var doorPublicKey = publicKeys[1];

        var value = 1;
        var loopSize = 0;

        while (value != cardPublicKey)
        {
            value = value * 7 % modulus;
            loopSize++;
        }

        return doorPublicKey.ModPow(loopSize, modulus).ToString();
    }

    public string SolveSecond()
    {
        return "Part2 was a click on the page";
    }
}