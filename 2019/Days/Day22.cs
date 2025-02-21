using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using System.Numerics;

namespace AdventOfCode._2019.Days;

public class Day22 : IRiddle
{
    public string SolveFirst()
    {
        const int toFind = 2019;
        const int cardAmount = 10007;

        var cards = Enumerable.Range(0, cardAmount)
            .ToList();

        foreach (var line in this.InputToLines())
        {
            var index = line.LastIndexOf(' ');
            var technique = line[..index];
            _ = int.TryParse(line[(index + 1)..], out var number);

            switch (technique)
            {
                case "deal with increment":
                    var newCards = new int[cardAmount];
                    var n = 0;
                    for (var i = 0; i < cardAmount; i++)
                    {
                        newCards[n % cardAmount] = cards[i];
                        n += number;
                    }

                    cards = newCards.ToList();
                    break;
                case "deal into new":
                    cards.Reverse();
                    break;
                case "cut":
                    cards = cards
                        .ToArray()
                        .Rotate((-number + cardAmount) % cardAmount)
                        .ToList();
                    break;
            }
        }

        return cards.IndexOf(toFind).ToString();
    }

    public string SolveSecond()
    {
        var toFind = new BigInteger(2020);
        var n = BigInteger.Parse("119315717514047");
        var m = BigInteger.Parse("101741582076661");

        var a = BigInteger.One;
        var b = BigInteger.Zero;

        foreach (var line in this.InputToLines())
        {
            var x = line.Split();
            if (x[0] == "cut")
            {
                var k = BigInteger.Parse(x[1]);
                b = (b - k) % n;
            }
            else
                switch (x[1])
                {
                    case "into":
                        a = -a % n;
                        b = (-b - 1) % n;
                        break;
                    case "with":
                        {
                            var k = BigInteger.Parse(x[3]);
                            a = a * k % n;
                            b = b * k % n;
                            break;
                        }
                }
        }

        var (ansA, ansB) = MatrixPower((a, b, 0, 1), m, n);

        return (InvPrime(ansA, n) * (toFind - ansB) % n).ToString();
    }

    private static (BigInteger, BigInteger, BigInteger, BigInteger) MatrixMultiplication((BigInteger, BigInteger, BigInteger, BigInteger) matA, (BigInteger, BigInteger, BigInteger, BigInteger) matB, BigInteger n)
    {
        return (
            (matA.Item1 * matB.Item1 + matA.Item2 * matB.Item3) % n,
            (matA.Item1 * matB.Item2 + matA.Item2 * matB.Item4) % n,
            (matA.Item3 * matB.Item1 + matA.Item4 * matB.Item3) % n,
            (matA.Item3 * matB.Item2 + matA.Item4 * matB.Item4) % n
        );
    }

    private static (BigInteger, BigInteger) MatrixPower((BigInteger, BigInteger, BigInteger, BigInteger) mat, BigInteger exp, BigInteger n)
    {
        var mul = mat;
        var ans = (BigInteger.One, BigInteger.Zero, BigInteger.Zero, BigInteger.One);
        while (exp > 0)
        {
            if (exp % 2 == 1)
            {
                ans = MatrixMultiplication(mul, ans, n);
            }
            exp /= 2;
            mul = MatrixMultiplication(mul, mul, n);
        }
        return (ans.Item1, ans.Item2);
    }

    private static BigInteger InvPrime(BigInteger num, BigInteger p)
    {
        var exp = p - 2;
        var mul = num;
        var ans = BigInteger.One;
        while (exp > 0)
        {
            if (exp % 2 == 1)
            {
                ans = ans * mul % p;
            }
            exp /= 2;
            mul = mul * mul % p;
        }
        return ans;
    }
}