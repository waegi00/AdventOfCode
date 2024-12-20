using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        var grid = new bool[6][];
        for (var i = 0; i < grid.Length; i++)
        {
            grid[i] = new bool[50];
        }

        foreach (var line in this.InputToLines())
        {
            var splits = line.Split(' ');
            if (splits[0] == "rect")
            {
                var pos = splits[1].Split('x').Select(int.Parse).ToList();
                grid.SetValues(0, 0, pos[1] - 1, pos[0] - 1, _ => true);
            }
            else if (splits[1] == "column")
            {
                var col = int.Parse(splits[2][2..]);
                var n = int.Parse(splits[^1]);
                grid.RotateCol(col, n);
            }
            else
            {
                var row = int.Parse(splits[2][2..]);
                var n = int.Parse(splits[^1]);
                grid.RotateRow(row, n);
            }
        }

        return grid.Sum(r => r.Count(c => c)).ToString();
    }

    public string SolveSecond()
    {
        var grid = new bool[6][];
        for (var i = 0; i < grid.Length; i++)
        {
            grid[i] = new bool[50];
        }

        foreach (var line in this.InputToLines())
        {
            var splits = line.Split(' ');
            if (splits[0] == "rect")
            {
                var pos = splits[1].Split('x').Select(int.Parse).ToList();
                grid.SetValues(0, 0, pos[1] - 1, pos[0] - 1, _ => true);
            }
            else if (splits[1] == "column")
            {
                var col = int.Parse(splits[2][2..]);
                var n = int.Parse(splits[^1]);
                grid.RotateCol(col, n);
            }
            else
            {
                var row = int.Parse(splits[2][2..]);
                var n = int.Parse(splits[^1]);
                grid.RotateRow(row, n);
            }
        }

        var charGrid = grid.Select(r => r.Select(c => c ? '#' : ' '));
        return "\r\n" + string.Join("\r\n", charGrid.Select(cs => new string(cs.ToArray())));
    }
}