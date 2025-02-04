using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        var magicNumber = int.Parse(this.InputToLines()[8].Split()[1]);

        return RunActivationSystem(magicNumber, true).ToString();
    }

    public string SolveSecond()
    {
        var magicNumber = int.Parse(this.InputToLines()[8].Split()[1]);

        return RunActivationSystem(magicNumber, false).ToString();
    }

    private static int RunActivationSystem(int magicNumber, bool isPart1)
    {
        var seen = new HashSet<int>();
        var c = 0;
        var lastUniqueC = -1;

        while (true)
        {
            var a = c | 65536;
            c = magicNumber;

            while (true)
            {
                c = (((c + (a & 255)) & 16777215) * 65899) & 16777215;

                if (a < 256)
                {
                    if (isPart1)
                    {
                        return c;
                    }

                    if (!seen.Add(c))
                    {
                        return lastUniqueC;
                    }

                    lastUniqueC = c;
                    break;

                }

                a /= 256;
            }
        }
    }
}