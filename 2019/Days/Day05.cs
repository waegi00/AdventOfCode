using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day05 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        const int systemId = 1;
        var output = new List<int>();

        for (var i = 0; i < input.Length;)
        {
            var n = input[i];
            var c = n / 100 % 10;
            var b = n / 1000 % 10;
            var a = n / 10000 % 10;
            switch (n % 100)
            {
                case 1:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] +
                        input[b == 0 ? input[i + 2] : i + 2];
                    i += 4;
                    break;
                case 2:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] *
                        input[b == 0 ? input[i + 2] : i + 2];
                    i += 4;
                    break;
                case 3:
                    input[c == 0 ? input[i + 1] : i + 1] = systemId;
                    i += 2;
                    break;
                case 4:
                    output.Add(input[c == 0 ? input[i + 1] : i + 1]);
                    i += 2;
                    break;
                case 99:
                    i = input.Length;
                    break;
            }
        }

        return output[..^1].All(x => x == 0)
            ? output[^1].ToString()
            : "";
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        const int systemId = 5;
        var output = new List<int>();

        for (var i = 0; i < input.Length;)
        {
            var n = input[i];
            var c = n / 100 % 10;
            var b = n / 1000 % 10;
            var a = n / 10000 % 10;
            switch (n % 100)
            {
                case 1:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] +
                        input[b == 0 ? input[i + 2] : i + 2];
                    i += 4;
                    break;
                case 2:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] *
                        input[b == 0 ? input[i + 2] : i + 2];
                    i += 4;
                    break;
                case 3:
                    input[c == 0 ? input[i + 1] : i + 1] = systemId;
                    i += 2;
                    break;
                case 4:
                    output.Add(input[c == 0 ? input[i + 1] : i + 1]);
                    i += 2;
                    break;
                case 5:
                    if (input[c == 0 ? input[i + 1] : i + 1] != 0)
                    {
                        i = input[b == 0 ? input[i + 2] : i + 2];
                    }
                    else
                    {
                        i += 3;
                    }
                    break;
                case 6:
                    if (input[c == 0 ? input[i + 1] : i + 1] == 0)
                    {
                        i = input[b == 0 ? input[i + 2] : i + 2];
                    }
                    else
                    {
                        i += 3;
                    }
                    break;
                case 7:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] < input[b == 0 ? input[i + 2] : i + 2]
                        ? 1
                        : 0;
                    i += 4;
                    break;
                case 8:
                    input[a == 0 ? input[i + 3] : i + 3] =
                        input[c == 0 ? input[i + 1] : i + 1] == input[b == 0 ? input[i + 2] : i + 2]
                            ? 1
                            : 0;
                    i += 4;
                    break;
                case 99:
                    i = input.Length;
                    break;
            }
        }

        return output[0].ToString();
    }
}