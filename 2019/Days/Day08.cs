using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day08 : IRiddle
{
    private const int width = 25;
    private const int height = 6;

    public string SolveFirst()
    {
        var input = this.InputToText()
            .ToCharArray()
            .Select(x => x - '0')
            .ToArray();

        var layers = new int[input.Length / width / height][][];

        for (var l = 0; l < layers.Length; l++)
        {
            layers[l] = new int[height][];

            for (var i = 0; i < height; i++)
            {
                var start = l * width * height + i * width;
                layers[l][i] = input[start..(start + width)];
            }
        }

        var maxLayer = layers.MinBy(x => x.Sum(y => y.Count(z => z == 0)))!;
        return (maxLayer.Sum(x => x.Count(y => y == 1)) *
                maxLayer.Sum(x => x.Count(y => y == 2)))
            .ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .ToCharArray()
            .Select(x => x - '0')
            .ToArray();

        var layers = new int[input.Length / width / height][][];

        for (var l = 0; l < layers.Length; l++)
        {
            layers[l] = new int[height][];

            for (var i = 0; i < height; i++)
            {
                var start = l * width * height + i * width;
                layers[l][i] = input[start..(start + width)];
            }
        }

        var image = Enumerable.Range(0, height)
            .Select(_ => new char[width])
            .ToArray();

        foreach (var layer in layers)
        {
            foreach (var (row, i) in layer.Select((x, i) => (x, i)))
            {
                foreach (var (pixel, j) in row.Select((x, i) => (x, i)))
                {
                    switch (pixel)
                    {
                        case 0:
                        case 1:
                            if (image[i][j] == '\0')
                            {
                                image[i][j] = pixel == 0 ? ' ' : 'X';
                            }
                            break;
                    }
                }
            }
        }

        image.Print();

        return "";
    }
}