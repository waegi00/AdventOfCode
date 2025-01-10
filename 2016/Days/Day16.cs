using System.Text;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2016.Days;

public class Day16 : IRiddle
{
    public string SolveFirst()
    {
        const int diskLength = 272;
        var state = this.InputToText();

        while (state.Length < diskLength)
        {
            var copy = new string(state.Replace('0', 'X').Replace('1', '0').Replace('X', '1').Reverse().ToArray());

            state += '0' + copy;
        }

        return Checksum(state[..diskLength]);
    }

    public string SolveSecond()
    {
        const int diskLength = 35651584;
        var state = this.InputToText();

        while (state.Length < diskLength)
        {
            var copy = new string(state.Replace('0', 'X').Replace('1', '0').Replace('X', '1').Reverse().ToArray());

            state += '0' + copy;
        }

        return Checksum(state[..diskLength]);
    }

    private static string Checksum(string str)
    {
        while (true)
        {
            if (str.Length % 2 == 1)
            {
                return str;
            }

            var strChecksum = new StringBuilder();
            for (var i = 0; i < str.Length; i += 2)
            {
                strChecksum.Append(str[i] == str[i + 1] ? '1' : '0');
            }

            str = strChecksum.ToString();
        }
    }
}
