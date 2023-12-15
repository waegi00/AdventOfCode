using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllText("Days\\Inputs\\Day15.txt");

        var sum = 0;

        foreach (var split in input.Split(","))
        {
            var c = 0;

            foreach (var s in split)
            {
                c += s;
                c *= 17;
                c %= 256;
            }

            sum += c;
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllText("Days\\Inputs\\Day15.txt");

        var boxes = new List<Step>[256];
        for (var i = 0; i < 256; i++) boxes[i] = new List<Step>();

        foreach (var split in input.Split(","))
        {
            var c = 0;

            var indexOfSign = split.IndexOf(split.Contains('=') ? '=' : '-', StringComparison.Ordinal);
            var name = split[..indexOfSign];

            foreach (var s in name)
            {
                c += s;
                c *= 17;
                c %= 256;
            }

            var box = boxes[c];

            switch (split[indexOfSign])
            {
                case '=':
                    var index = box.FindIndex(x => x.Name == name);
                    if (index != -1)
                    {
                        box[index] = new Step(name, long.Parse(split[indexOfSign + 1].ToString()));
                    }
                    else
                    {
                        box.Add(new Step(name, long.Parse(split[indexOfSign + 1].ToString())));
                    }

                    break;
                case '-':
                    boxes[c].RemoveAll(x => x.Name == name);
                    break;
            }

        }

        return boxes.SelectMany((box, i) => box.Select((step, j) => (i + 1) * (j + 1) * step.FocalLength)).Sum().ToString();
    }

    private record Step(string Name, long FocalLength);
}