using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        input[1] = 12;
        input[2] = 2;

        for (var i = 0; i < input.Length; i += 4)
        {
            var n = input[i];
            switch (n)
            {
                case 1:
                    input[input[i + 3]] =
                        input[input[i + 1]] + input[input[i + 2]];
                    break;
                case 2:
                    input[input[i + 3]] =
                        input[input[i + 1]] * input[input[i + 2]];
                    break;
                case 99:
                    i = input.Length;
                    break;
            }
        }
        return input[0].ToString();
    }

    public string SolveSecond()
    {
        const int goal = 19690720;

        var input = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        for (var noun = 0; noun < 99; noun++)
        {
            for (var verb = 0; verb < 99; verb++)
            {
                var data = input.ToArray();
                data[1] = noun;
                data[2] = verb;

                for (var i = 0; i < data.Length; i += 4)
                {
                    var n = data[i];
                    switch (n)
                    {
                        case 1:
                            data[data[i + 3]] =
                                data[data[i + 1]] + data[data[i + 2]];
                            break;
                        case 2:
                            data[data[i + 3]] =
                                data[data[i + 1]] * data[data[i + 2]];
                            break;
                        case 99:
                            i = data.Length;
                            break;
                    }
                }

                if (data[0] == goal)
                {
                    return (100 * noun + verb).ToString();
                }
            }
        }

        return "";
    }
}