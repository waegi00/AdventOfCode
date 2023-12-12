using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day11 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day11.txt");

        var galaxies = new List<(long, long)>();
        var emptyRows = Enumerable.Range(1, input.Length).Select(x => (long)x).ToList();
        var emptyColumns = Enumerable.Range(1, input[0].Length).Select(x => (long)x).ToList();


        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '#') continue;
                galaxies.Add((i, j));
                emptyRows.Remove(i);
                emptyColumns.Remove(j);
            }
        }

        long sum = 0;

        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                var g1 = galaxies[i];
                var g2 = galaxies[j];
                sum += (long)Math.Abs(g1.Item1 - g2.Item1) + (long)Math.Abs(g1.Item2 - g2.Item2) + Between(g1.Item1, g2.Item1, emptyRows) + Between(g1.Item2, g2.Item2, emptyColumns);
            }
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day11.txt");

        var galaxies = new List<(long, long)>();
        var emptyRows = Enumerable.Range(1, input.Length).Select(x => (long)x).ToList();
        var emptyColumns = Enumerable.Range(1, input[0].Length).Select(x => (long)x).ToList();


        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '#') continue;
                galaxies.Add((i, j));
                emptyRows.Remove(i);
                emptyColumns.Remove(j);
            }
        }

        long sum = 0;

        for (var i = 0; i < galaxies.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                var g1 = galaxies[i];
                var g2 = galaxies[j];
                sum += (long)Math.Abs(g1.Item1 - g2.Item1) + (long)Math.Abs(g1.Item2 - g2.Item2) + Between(g1.Item1, g2.Item1, emptyRows, 999999) + Between(g1.Item2, g2.Item2, emptyColumns, 999999);
            }
        }

        return sum.ToString();
    }

    private static long Between(long start, long end, IEnumerable<long> numbers, long factor = 1)
    {
        if (start > end)
        {
            (start, end) = (end, start);
        }

        return numbers.Count(x => x > start && x < end) * factor;
    }
}
