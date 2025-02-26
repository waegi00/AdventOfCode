using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var start = int.Parse(input[0]);
        var buses = input[1]
            .Split(',')
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToArray();

        var bus = buses.MinBy(x => x - start % x);
        return (bus * (bus - start % bus)).ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();
        var buses = input[1].Split(',');

        var bi = new List<long>();
        var ni = new List<long>();

        for (var i = 0; i < buses.Length; i++)
        {
            if (buses[i] == "x") continue;
            var busId = long.Parse(buses[i]);
            bi.Add((busId - i % busId) % busId);
            ni.Add(busId);
        }

        var factorial = ni.Product();

        var Ni = ni.Select(n => factorial / n).ToList();
        var xi = new List<long>();

        for (var i = 0; i < Ni.Count; i++)
        {
            var num = Ni[i] % ni[i];
            var x = 1L;
            while (true)
            {
                if (x * num % ni[i] == 1)
                {
                    break;
                }
                x++;
            }
            xi.Add(x);
        }
        
        return (bi.Select((t, i) => t * Ni[i] * xi[i]).Sum() % factorial).ToString();
    }
}