using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day03 : IRiddle
{
    public string SolveFirst()
    {
        var lines = this.InputToLines();
        var bits = new int[lines[0].Length];

        foreach (var line in lines)
        {
            foreach (var (item, i) in line.WithIndex())
            {
                if (item == '0')
                {
                    bits[i]--;
                }
                else
                {
                    bits[i]++;
                }
            }
        }

        var gammaRate = Convert.ToInt32(new string(bits.Select(b => b > 0 ? '1' : '0').ToArray()), 2);

        return (gammaRate * (gammaRate ^ ((1 << bits.Length) - 1))).ToString();
    }

    public string SolveSecond()
    {
        var lines = this.InputToLines();

        var oxygenLines = lines.ToArray();
        var i = 0;
        while (oxygenLines.Length > 1)
        {
            var bits = oxygenLines.Select(x => x[i]).ToArray();

            var bit = bits.Count(x => x == '0') * 2 > bits.Length ? '0' : '1';
            oxygenLines = oxygenLines.Where(x => x[i] == bit).ToArray();

            i++;
        }

        var co2ScrubberLines = lines.ToArray();
        i = 0;
        while (co2ScrubberLines.Length > 1)
        {
            var bits = co2ScrubberLines.Select(x => x[i]).ToArray();

            var bit = bits.Count(x => x == '1') * 2 < bits.Length ? '1' : '0';
            co2ScrubberLines = co2ScrubberLines.Where(x => x[i] == bit).ToArray();

            i++;
        }

        return (Convert.ToInt32(oxygenLines[0], 2) * Convert.ToInt32(co2ScrubberLines[0], 2)).ToString();
    }
}