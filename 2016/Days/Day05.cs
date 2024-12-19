using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Interfaces;

namespace AdventOfCode._2016.Days;

public class Day05 : IRiddle
{
    private const string _input = "ffykfhsq";

    public string SolveFirst()
    {
        var res = new StringBuilder();

        var n = 1;
        while (res.Length < 8)
        {
            var inputBytes = Encoding.ASCII.GetBytes(_input + n);
            var hashBytes = MD5.HashData(inputBytes);
            var hashString = Convert.ToHexString(hashBytes);

            if (hashString.StartsWith("00000"))
            {
                res.Append(hashString[5]);
            }
            n++;
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var res = new char[8];

        var n = 1;
        while (res.Any(r => r == default))
        {
            var inputBytes = Encoding.ASCII.GetBytes(_input + n);
            var hashBytes = MD5.HashData(inputBytes);
            var hashString = Convert.ToHexString(hashBytes);

            if (hashString.StartsWith("00000"))
            {
                if (int.TryParse(hashString[5].ToString(), out var pos) && 
                    pos < res.Length &&
                    res[pos] == default)
                {
                    res[pos] = hashString[6];
                }
            }
            n++;
        }

        return new string(res);
    }
}
