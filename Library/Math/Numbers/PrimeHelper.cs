namespace AdventOfCode.Library.Math.Numbers;

public static class PrimeHelper
{
    /// <summary>
    /// Returns all divisors of n
    /// </summary>
    /// <param name="n">Number to get all divisors for</param>
    /// <returns>IEnumerable of divisors or [] for n smaller than 1</returns>
    public static IEnumerable<int> Divisors(int n)
    {
        if (n <= 0) yield break;

        for (var i = 1; i <= System.Math.Sqrt(n); i++)
        {
            if (n % i != 0) continue;
            yield return i;
            if (i != n / i)
            {
                yield return n / i;
            }
        }
    }
}