using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2022.Days;

public class Day20 : IRiddle
{
    public string SolveFirst()
    {
        return Solve();
    }

    public string SolveSecond()
    {
        return Solve(811589153, 10);
    }

    private string Solve(long decryptionKey = 1, int numberOfMixes = 1)
    {
        return GetEncryptedCoordinates(GetInitialFile(this.InputToLines(), decryptionKey), numberOfMixes).ToString();
    }

    private static List<(long item, int index)> GetInitialFile(string[] input, long decryptionKey)
    {
        return input.WithIndex()
            .Select(x => (long.Parse(x.item) * decryptionKey, x.index))
            .ToList();
    }

    private static long GetEncryptedCoordinates(List<(long item, int index)> initialList, int numberOfMixes)
    {
        var numbers = new List<(long item, int index)>(initialList);
        var length = numbers.Count - 1;
        for (var i = 0; i < numberOfMixes; i++)
        {
            foreach (var (item, index) in initialList)
            {
                var pos = numbers.IndexOf((item, index));
                var newPos = (int)(pos + item).Mod(length);

                numbers.RemoveAt(pos);
                numbers.Insert(newPos, (item, index));
            }
        }

        var zeroPos = numbers.FindIndex(i => i.item == 0);

        return Enumerable.Range(1, 3)
            .Sum(n => numbers[(zeroPos + n * 1000) % numbers.Count].item);
    }
}
