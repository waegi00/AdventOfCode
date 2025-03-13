using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day19 : IRiddle
{

    private static readonly List<(int, int, int)> CoordinateRemaps = [(0, 1, 2), (0, 2, 1), (1, 0, 2), (1, 2, 0), (2, 0, 1), (2, 1, 0)];
    private static readonly List<(int, int, int)> CoordinateNegations = [(1, 1, 1), (1, 1, -1), (1, -1, 1), (1, -1, -1), (-1, 1, 1), (-1, 1, -1), (-1, -1, 1), (-1, -1, -1)];
    private static readonly List<(int, int, int)> DistancesFromScan0 = [(0, 0, 0)];

    public string SolveFirst()
    {
        var data = ReadInput(this.InputToText());

        var alignedIndices = new HashSet<int> { 0 };
        var aligned = new Dictionary<int, List<List<int>>> { { 0, data[0] } };
        var allAligned = new HashSet<(int, int, int)>(data[0].Select(x => (x[0], x[1], x[2])));
        var noAlign = new HashSet<(int, int)>();

        while (alignedIndices.Count < data.Count)
        {
            for (var i = 0; i < data.Count; i++)
            {
                if (alignedIndices.Contains(i)) continue;

                foreach (var j in alignedIndices.Where(j => !noAlign.Contains((i, j))))
                {
                    var (ok, remap) = FindAlignment(aligned[j], data[i]);
                    if (ok && remap != null)
                    {
                        alignedIndices.Add(i);
                        aligned[i] = remap;
                        allAligned.UnionWith(remap.Select(x => (x[0], x[1], x[2])));
                        break;
                    }
                    noAlign.Add((i, j));
                }
            }
        }

        return allAligned.Count.ToString();
    }

    public string SolveSecond()
    {
        var data = ReadInput(this.InputToText());

        var alignedIndices = new HashSet<int> { 0 };
        var aligned = new Dictionary<int, List<List<int>>> { { 0, data[0] } };
        var noAlign = new HashSet<(int, int)>();

        while (alignedIndices.Count < data.Count)
        {
            for (var i = 0; i < data.Count; i++)
            {
                if (alignedIndices.Contains(i)) continue;

                foreach (var j in alignedIndices.Where(j => !noAlign.Contains((i, j))))
                {
                    var (ok, remap) = FindAlignment(aligned[j], data[i]);
                    if (ok && remap != null)
                    {
                        alignedIndices.Add(i);
                        aligned[i] = remap;
                        break;
                    }
                    noAlign.Add((i, j));
                }
            }
        }

        return DistancesFromScan0.Aggregate(0, (current, a) =>
                DistancesFromScan0.Select(b =>
                        Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2) + Math.Abs(a.Item3 - b.Item3))
                    .Prepend(current)
                    .Max())
            .ToString();
    }

    private static List<List<List<int>>> ReadInput(string input)
    {
        return input.Split("\r\n\r\n")
            .Select(l =>
                l.Split("\r\n")
                    .Skip(1)
                    .Select(x =>
                        x.Split(',')
                            .Select(int.Parse)
                            .ToList())
                    .ToList())
            .ToList();
    }

    private static List<List<int>> Apply((int, int, int) remap, (int, int, int) negation, List<List<int>> scan)
    {
        return scan
            .Select(item =>
                new List<int> { negation.Item1 * item[remap.Item1], negation.Item2 * item[remap.Item2], negation.Item3 * item[remap.Item3] })
            .ToList();
    }


    private static (bool, List<List<int>>?) FindAlignment(List<List<int>> scanA, List<List<int>> scanB)
    {
        var inA = new HashSet<(int, int, int)>(scanA.Select(x => (x[0], x[1], x[2])));

        foreach (var remap in CoordinateRemaps)
        {
            foreach (var negation in CoordinateNegations)
            {
                var b = Apply(remap, negation, scanB);
                foreach (var aPos in scanA)
                {
                    foreach (var bPos in b)
                    {
                        var remapBy = (bPos[0] - aPos[0], bPos[1] - aPos[1], bPos[2] - aPos[2]);
                        var matches = 0;
                        var allRemapped = new List<List<int>>();

                        foreach (var remappedToA in b.Select(otherB => (otherB[0] - remapBy.Item1, otherB[1] - remapBy.Item2, otherB[2] - remapBy.Item3)))
                        {
                            if (inA.Contains(remappedToA))
                            {
                                matches++;
                            }
                            allRemapped.Add([remappedToA.Item1, remappedToA.Item2, remappedToA.Item3]);
                        }

                        if (matches < 12) continue;
                        DistancesFromScan0.Add(remapBy);
                        return (true, allRemapped);
                    }
                }
            }
        }

        return (false, null);
    }
}