using System.Numerics;

namespace AdventOfCode.Library.Math.Numbers;

public static class NumberHelper
{
    public static T Mod<T>(this T x, T m) where T : INumber<T>
    {
        return (x % m + m) % m;
    }
}