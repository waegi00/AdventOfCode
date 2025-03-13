using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day16 : IRiddle
{
    private static int pos;
    private static int ver;
    private static string? bits;
    private static readonly Func<long, long, long>[] operations =
    [
        (a, b) => a + b,
        (a, b) => a * b,
        Math.Min,
        Math.Max,
        null!,
        (a, b) => a > b ? 1 : 0,
        (a, b) => a < b ? 1 : 0,
        (a, b) => a == b ? 1 : 0
    ];

    public string SolveFirst()
    {
        Setup();
        Packet();
        return ver.ToString();
    }

    public string SolveSecond()
    {
        Setup(); 
        return Packet().ToString();
    }

    private void Setup()
    {
        var input = this.InputToText();
        bits = string.Join("", input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        pos = ver = 0;
    }

    private static int Read(int size)
    {
        var value = Convert.ToInt32(bits?.Substring(pos, size), 2);
        pos += size;
        return value;
    }

    private static long Packet()
    {
        ver += Read(3);
        var typeId = Read(3);

        if (typeId == 4)
        {
            long total = 0;
            int go;
            do
            {
                go = Read(1);
                total = (total << 4) | (uint)Read(4);
            } while (go == 1);
            return total;
        }
        else
        {
            long total;
            if (Read(1) == 0)
            {
                var length = Read(15) + pos;
                total = Packet();
                while (pos < length)
                {
                    total = operations[typeId](total, Packet());
                }
            }
            else
            {
                var count = Read(11);
                total = Packet();
                for (var i = 1; i < count; i++)
                {
                    total = operations[typeId](total, Packet());
                }
            }
            return total;
        }
    }
}