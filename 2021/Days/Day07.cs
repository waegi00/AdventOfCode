using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;

namespace AdventOfCode._2021.Days;

public class Day07 : IRiddle
{
    public string SolveFirst()
    {
        var crabs = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();
        
        var median = crabs.Median();

        return crabs.Sum(c => Math.Abs(c - median)).ToString();
    }

    public string SolveSecond()
    {
        var crabs = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        var mean = (int)crabs.Average();

        return crabs.Sum(c => GaussSummation(Math.Abs(c - mean))).ToString();
    }


    private static int GaussSummation(int n) => n * (n + 1) / 2;
}