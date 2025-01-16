using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2017.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText()
            .Split(',');

        var programs = Enumerable.Range(0, 'p' - 'a' + 1).Select(x => (char)('a' + x)).ToArray();

        foreach (var line in input)
        {
            switch (line[0])
            {
                case 's':
                    var x = int.Parse(line[1..]);
                    programs = programs.Rotate(x);
                    break;
                case 'x':
                    var splits = line[1..]
                        .Split('/')
                        .Select(int.Parse)
                        .ToArray();

                    (programs[splits[0]], programs[splits[1]]) = (programs[splits[1]], programs[splits[0]]);
                    break;
                case 'p':
                    var partners = line[1..]
                        .Split('/')
                        .Select(y => Array.IndexOf(programs, y[0]))
                        .ToArray();

                    (programs[partners[0]], programs[partners[1]]) = (programs[partners[1]], programs[partners[0]]);
                    break;
            }
        }

        return new string(programs);
    }

    public string SolveSecond()
    {
        var input = this.InputToText()
            .Split(',');

        var programs = Enumerable.Range(0, 'p' - 'a' + 1).Select(x => (char)('a' + x)).ToArray();

        var cache = new Dictionary<string, int>();
        const int length = 1000000000;
        for (var i = 1; i <= length; i++)
        {
            foreach (var line in input)
            {
                switch (line[0])
                {
                    case 's':
                        var x = int.Parse(line[1..]);
                        programs = programs.Rotate(x);
                        break;
                    case 'x':
                        var splits = line[1..]
                            .Split('/')
                            .Select(int.Parse)
                            .ToArray();

                        (programs[splits[0]], programs[splits[1]]) = (programs[splits[1]], programs[splits[0]]);
                        break;
                    case 'p':
                        var partners = line[1..]
                            .Split('/')
                            .Select(y => Array.IndexOf(programs, y[0]))
                            .ToArray();

                        (programs[partners[0]], programs[partners[1]]) = (programs[partners[1]], programs[partners[0]]);
                        break;
                }
            }

            var curr = new string(programs);
            if (cache.TryGetValue(curr, out var index))
            {
                var cycleLength = i - index;
                var offset = (length - i) % cycleLength;

                return cache.First(x => x.Value == index + offset).Key;
            }

            cache[curr] = i;
        }

        return new string(programs);
    }
}