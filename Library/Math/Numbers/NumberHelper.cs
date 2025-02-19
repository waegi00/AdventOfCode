using System.Numerics;

namespace AdventOfCode.Library.Math.Numbers;

public static class NumberHelper
{
    /// <summary>
    /// Calculates the modulus of two numbers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x">Dividend</param>
    /// <param name="m">Divisor</param>
    /// <returns>Modulus of the numbers</returns>
    public static T Mod<T>(this T x, T m) where T : INumber<T>
    {
        return (x % m + m) % m;
    }

    /// <summary>
    /// Calculates Least common multiple of two numbers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns>Least common multiple of the numbers</returns>
    public static T LCM<T>(this T a, T b) where T : INumber<T>
    {
        return a * b / a.GCD(b);
    }

    /// <summary>
    /// Calculates Greatest common divisor of two numbers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns>Greatest common divisor of the numbers</returns>
    public static T GCD<T>(this T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}