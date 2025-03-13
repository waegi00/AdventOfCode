using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day20 : IRiddle
{

    public string SolveFirst()
    {
        return Enhance(2).ToString();
    }

    public string SolveSecond()
    {
        return Enhance(50).ToString();
    }

    private int Enhance(int times)
    {
        var data = this.InputToLines();
        var algo = data[0];
        var A = data.Skip(2).Where(l => !string.IsNullOrEmpty(l)).Select(l => l.Select(c => c == '#' ? 1 : 0).ToArray()).ToArray();

        var algoArr = algo.Select(c => c == '#' ? 1 : 0).ToArray();
        var defaults = new[] { algoArr[0], algoArr[0] != 0 ? algoArr[511] : algoArr[0] };

        return Enhance(A, algoArr, defaults, times).Sum(r => r.Sum());
    }

    private static int[][] Enhance(int[][] A, int[] algo, int[] defaults, int times)
    {
        for (var it = 0; it < times; it++)
        {
            A = Pad(A, defaults[(it + 1) % 2]);
            var B = A.Select(row => row.Select(_ => defaults[it % 2]).ToArray()).ToArray();

            for (var i = 1; i < A.Length - 1; i++)
            {
                for (var j = 1; j < A[i].Length - 1; j++)
                {
                    var index = 0;
                    for (var di = -1; di <= 1; di++)
                    {
                        for (var dj = -1; dj <= 1; dj++)
                        {
                            index = (index << 1) | A[i + di][j + dj];
                        }
                    }
                    B[i][j] = algo[index];
                }
            }
            A = B;
        }
        return A;
    }

    private static int[][] Pad(int[][] A, int value)
    {
        var rows = A.Length + 4;
        var cols = A[0].Length + 4;
        var padded = Enumerable.Range(0, rows).Select(_ => Enumerable.Repeat(value, cols).ToArray()).ToArray();

        for (var i = 0; i < A.Length; i++)
        {
            Array.Copy(A[i], 0, padded[i + 2], 2, A[i].Length);
        }
        return padded;
    }
}