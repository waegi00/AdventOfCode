using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day24 : IRiddle
{
    public string SolveFirst()
    {
        var tiles = new HashSet<(int q, int r)>();

        foreach (var l in this.InputToLines())
        {
            var q = 0;
            var r = 0;
            var i = 0;

            while (i < l.Length)
            {
                switch (l[i])
                {
                    case 'e': i += 1; q += 1; break;
                    case 'w': i += 1; q -= 1; break;
                    default:
                        {
                            switch (l[i..(i + 2)])
                            {
                                case "ne": q += 1; r -= 1; break;
                                case "nw": r -= 1; break;
                                case "se": r += 1; break;
                                case "sw": q -= 1; r += 1; break;
                            }

                            i += 2;
                            break;
                        }
                }
            }

            if (!tiles.Add((q, r)))
            {
                tiles.Remove((q, r));
            }
        }

        return tiles.Count.ToString();
    }

    public string SolveSecond()
    {

        var tiles = new HashSet<(int q, int r)>();

        foreach (var l in this.InputToLines())
        {
            var q = 0;
            var r = 0;
            var i = 0;

            while (i < l.Length)
            {
                switch (l[i])
                {
                    case 'e': i += 1; q += 1; break;
                    case 'w': i += 1; q -= 1; break;
                    default:
                        {
                            switch (l[i..(i + 2)])
                            {
                                case "ne": q += 1; r -= 1; break;
                                case "nw": r -= 1; break;
                                case "se": r += 1; break;
                                case "sw": q -= 1; r += 1; break;
                            }

                            i += 2;
                            break;
                        }
                }
            }

            if (!tiles.Add((q, r)))
            {
                tiles.Remove((q, r));
            }
        }

        for (var i = 0; i < 100; i++)
        {
            var newTiles = new HashSet<(int q, int r)>();

            var minQ = tiles.Min(x => x.q);
            var minR = tiles.Min(x => x.r);
            var maxQ = tiles.Max(x => x.q);
            var maxR = tiles.Max(x => x.r);

            for (var q = minQ - 1; q <= maxQ + 1; q++)
            {
                for (var r = minR - 1; r <= maxR + 1; r++)
                {
                    var adjacentCells = new[]
                    {
                        (q + 1, r), (q + 1, r - 1),
                        (q - 1, r), (q - 1, r + 1),
                        (q, r - 1), (q, r + 1)
                    };

                    var adjacent = adjacentCells.Count(tiles.Contains);
                    var isBlack = tiles.Contains((q, r));
                    if (isBlack && adjacent is 1 or 2 || !isBlack && adjacent == 2)
                    {
                        newTiles.Add((q, r));
                    }
                }
            }

            tiles = newTiles;
        }

        return tiles.Count.ToString();
    }
}