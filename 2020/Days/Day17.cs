using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;
using AdventOfCode.Library.String;

namespace AdventOfCode._2020.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        var active = new HashSet<(int x, int y, int z)>();

        foreach (var (data, x) in this.InputToLines().ToCharArray().WithIndex())
        {
            foreach (var (c, y) in data.WithIndex())
            {
                if (c == '#')
                {
                    active.Add((x, y, 0));
                }
            }
        }

        for (var i = 0; i < 6; i++)
        {
            var minX = active.Min(x => x.x) - 1;
            var minY = active.Min(x => x.y) - 1;
            var minZ = active.Min(x => x.z) - 1;
            var maxX = active.Max(x => x.x) + 1;
            var maxY = active.Max(x => x.y) + 1;
            var maxZ = active.Max(x => x.z) + 1;

            var next = new HashSet<(int x, int y, int z)>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    for (var z = minZ; z <= maxZ; z++)
                    {
                        var isActive = active.Contains((x, y, z));
                        var n = 0;

                        for (var dx = -1; dx <= 1 && n < 4; dx++)
                        {
                            for (var dy = -1; dy <= 1 && n < 4; dy++)
                            {
                                for (var dz = -1; dz <= 1 && n < 4; dz++)
                                {
                                    if (dx == 0 && dy == 0 && dz == 0) continue;
                                    if (active.Contains((x + dx, y + dy, z + dz)))
                                    {
                                        n++;
                                    }
                                }
                            }
                        }

                        if (isActive && n.IsBetween(2, 3) || !isActive && n == 3)
                        {
                            next.Add((x, y, z));
                        }
                    }
                }
            }

            active = next;

        }

        return active.Count.ToString();
    }

    public string SolveSecond()
    {
        var active = new HashSet<(int x, int y, int z, int w)>();

        foreach (var (data, x) in this.InputToLines().ToCharArray().WithIndex())
        {
            foreach (var (c, y) in data.WithIndex())
            {
                if (c == '#')
                {
                    active.Add((x, y, 0, 0));
                }
            }
        }

        for (var i = 0; i < 6; i++)
        {
            var minX = active.Min(x => x.x) - 1;
            var minY = active.Min(x => x.y) - 1;
            var minZ = active.Min(x => x.z) - 1;
            var minW = active.Min(x => x.w) - 1;
            var maxX = active.Max(x => x.x) + 1;
            var maxY = active.Max(x => x.y) + 1;
            var maxZ = active.Max(x => x.z) + 1;
            var maxW = active.Max(x => x.w) + 1;

            var next = new HashSet<(int x, int y, int z, int w)>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    for (var z = minZ; z <= maxZ; z++)
                    {
                        for (var w = minW; w <= maxW; w++)
                        {
                            var isActive = active.Contains((x, y, z, w));
                            var n = 0;

                            for (var dx = -1; dx <= 1 && n < 4; dx++)
                            {
                                for (var dy = -1; dy <= 1 && n < 4; dy++)
                                {
                                    for (var dz = -1; dz <= 1 && n < 4; dz++)
                                    {
                                        for (var dw = -1; dw <= 1 && n < 4; dw++)
                                        {
                                            if (dx == 0 && dy == 0 && dz == 0 && dw == 0) continue;
                                            if (active.Contains((x + dx, y + dy, z + dz, w + dw)))
                                            {
                                                n++;
                                            }
                                        }
                                    }
                                }
                            }

                            if (isActive && n.IsBetween(2, 3) || !isActive && n == 3)
                            {
                                next.Add((x, y, z, w));
                            }
                        }
                    }
                }
            }

            active = next;

        }

        return active.Count.ToString();
    }
}