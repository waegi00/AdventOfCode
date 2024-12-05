using System.Security.Cryptography;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days
{
    public class Day04 : IRiddle
    {
        public string SolveFirst()
        {
            var key = this.InputToText();

            using var md5 = MD5.Create();

            var i = 0;
            while (++i > 0)
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(key + i);
                var hashBytes = md5.ComputeHash(inputBytes);
                var hashString = Convert.ToHexString(hashBytes);

                if (hashString.StartsWith("00000"))
                {
                    return i.ToString();
                }
            }

            return "";
        }

        public string SolveSecond()
        {
            var key = this.InputToText();

            using var md5 = MD5.Create();

            var i = 0;
            while (++i > 0)
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(key + i);
                var hashBytes = md5.ComputeHash(inputBytes);
                var hashString = Convert.ToHexString(hashBytes);

                if (hashString.StartsWith("000000"))
                {
                    return i.ToString();
                }
            }

            return "";
        }
    }
}
