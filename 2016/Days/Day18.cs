using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day18 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(40);
    }

    public string SolveSecond()
    {
        return Solve(400000);
    }

    private string Solve(int rows)
    {
        List<List<bool>> input = [this.InputToText().Select(x => x == '^').ToList()];

        for (var i = 0; i < rows - 1; i++)
        {
            var row = new List<bool>();

            for (var j = 0; j < input[i].Count; j++)
            {
                var l = j != 0 && input[i][j - 1];
                var r = j != input[i].Count - 1 && input[i][j + 1];
                row.Add(l ^ r);
            }

            input.Add(row);
        }

        return input.SelectMany(x => x).Count(x => !x).ToString();
    }
}