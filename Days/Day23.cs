using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day23 : IRiddle
{
    private const char _path = '.';
    private const char _visited = 'O';
    private const char _start = 'S';
    private const char _slopeUp = '^';
    private const char _slopeLeft = '<';
    private const char _slopeDown = 'v';
    private const char _slopeRight = '>';

    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day23.txt").Select(x => x.ToCharArray()).ToArray();

        var y = Array.FindIndex(input[0], x => x == _path);
        input[0][y] = _start;

        return PathLength1(input, 1, y).Max().ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day23.txt").Select(x => x.ToCharArray()).ToArray();

        // Make graph, remove vertices with only two edges and connect the two edges
        // A         => A 
        // B     H   => F H
        // C D E F G => G
        return "";
    }

    // Assuming slopes cant be on the border
    public static List<int> PathLength1(char[][] arr, int x, int y)
    {
        arr[x][y] = _visited;

        var wayLengths = new List<int>();

        if (x == arr.Length - 1)
        {
            wayLengths.Add(arr.Sum(a => a.Count(x => x == _visited)));
        }

        if (x > 0 && arr[x - 1][y] is _path or _slopeUp)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x - 1, y));
        }

        if (y > 0 && arr[x][y - 1] is _path or _slopeLeft)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x, y - 1));
        }

        if (x < arr.Length - 1 && arr[x + 1][y] is _path or _slopeDown)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x + 1, y));
        }

        if (y < arr[x].Length - 1 && arr[x][y + 1] is _path or _slopeRight)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x, y + 1));
        }

        return wayLengths;
    }
}
