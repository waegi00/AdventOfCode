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
    /// Calculates Pow(baseValue, exponent) % modulus
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="baseValue">Base</param>
    /// <param name="exponent">Exponent</param>
    /// <param name="modulus">Modulus</param>
    /// <returns>(baseValue ^ exponent) % modulus</returns>
    public static T ModPow<T>(this T baseValue, T exponent, T modulus) where T : INumber<T>
    {
        var result = T.One;
        var two = T.One + T.One;

        while (exponent > T.Zero)
        {
            if (!(exponent / two * two == exponent))
            {
                result = result * baseValue % modulus;
            }
            baseValue = baseValue * baseValue % modulus;
            exponent /= two;
        }
        return result;
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

    /// <summary>
    /// Checks if a number is between two others
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item">Number to check</param>
    /// <param name="start">Lower bound</param>
    /// <param name="end">Upper bound</param>
    /// <returns>True if item is between lower and upper (inclusive)</returns>
    public static bool IsBetween<T>(this T item, T start, T end) where T : INumber<T>
    {
        return item >= start && item <= end;
    }
}