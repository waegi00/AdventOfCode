using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day23 : IRiddle
{
    private const char Path = '.';
    private const char Visited = 'O';
    private const char Start = 'S';
    private const char SlopeUp = '^';
    private const char SlopeLeft = '<';
    private const char SlopeDown = 'v';
    private const char SlopeRight = '>';

    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day23.txt").Select(x => x.ToCharArray()).ToArray();

        var y = Array.FindIndex(input[0], x => x == Path);
        input[0][y] = Start;

        return PathLength1(input, 1, y).Max().ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day23.txt").Select(x => x.ToCharArray()).ToArray();

        // Make graph, remove vertices with only two edges and connect the two edges
        // A         => A 
        // B     H   => F H
        // C D E F G => G
        return "";
    }

    // Assuming slopes cant be on the border
    public static List<int> PathLength1(char[][] arr, int x, int y)
    {
        arr[x][y] = Visited;

        var wayLengths = new List<int>();

        if (x == arr.Length - 1)
        {
            wayLengths.Add(arr.Sum(a => a.Count(x => x == Visited)));
        }

        if (x > 0 && arr[x - 1][y] is Path or SlopeUp)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x - 1, y));
        }

        if (y > 0 && arr[x][y - 1] is Path or SlopeLeft)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x, y - 1));
        }

        if (x < arr.Length - 1 && arr[x + 1][y] is Path or SlopeDown)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x + 1, y));
        }

        if (y < arr[x].Length - 1 && arr[x][y + 1] is Path or SlopeRight)
        {
            wayLengths.AddRange(PathLength1(arr.Select(a => a.ToArray()).ToArray(), x, y + 1));
        }

        return wayLengths;
    }
}
