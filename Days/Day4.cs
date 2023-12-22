using AdventOfCode2023.Days.Interfaces;

namespace AdventOfCode2023.Days;

public class Day4 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day4.txt");

        var sum = (
            from line in input 
            select line.Split("|") into splits 
            let winners = splits[0].Split(":")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim())) 
            let picked = splits[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim())) 
            select GetPoints(winners, picked)).Sum();

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("Days\\Inputs\\Day4.txt");

        var dic = new Dictionary<int, int>();

        for (var i = 0; i < input.Length; i++)
        {
            dic.Add(i, 1);
        }

        for (var i = 0; i < input.Length; i++)
        {
            var splits = input[i].Split("|");
            var winners = splits[0].Split(":")[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim()));
            var picked = splits[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim()));

            var c = GetMatches(winners, picked);

            while (c > 0)
            {
                try
                {
                    dic[c + i] += dic[i];
                }
                catch (Exception)
                {
                    // Ignore as no card over the length of the input can be won
                }
                finally
                {
                    c--;
                }
            }
        }

        return dic.Values.Sum().ToString();
    }

    private static int GetPoints(IEnumerable<int> winners, IEnumerable<int> picked)
    {
        var c = picked.Intersect(winners).Count();
        return c == 0 ? 0 : 1 << (c - 1);
    }

    private static int GetMatches(IEnumerable<int> winners, IEnumerable<int> picked)
    {
        return picked.Intersect(winners).Count();
    }
}
