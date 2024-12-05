using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day08 : IRiddle
{
    public string SolveFirst()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day8.txt");

        var directions = input[0].Trim();
        var nodes = new List<Node>();

        for (var i = 2; i < input.Length; i++)
        {
            nodes.Add(new Node { Name = input[i].Split('=')[0].Trim() });
        }

        for (var i = 2; i < input.Length; i++)
        {
            var node = nodes.FirstOrDefault(x => x.Name == input[i].Split('=')[0].Trim())!;
            var others = input[i].Replace("(", "").Replace(")", "").Trim().Split('=')[1].Split(",");
            node.Left = nodes.FirstOrDefault(x => x.Name == others[0].Trim())!;
            node.Right = nodes.FirstOrDefault(x => x.Name == others[1].Trim())!;
        }

        long steps = 0;
        var index = 0;
        var currentNode = nodes.FirstOrDefault(x => x.Name == "AAA")!;
        while (currentNode.Name != "ZZZ")
        {
            currentNode = directions[index] == 'R' ? currentNode.Right : currentNode.Left;
            index++;
            index %= directions.Length;
            steps++;
        }
        return steps.ToString();
    }

    public string SolveSecond()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day8.txt");

        var directions = input[0].Trim();
        var nodes = new List<Node>();

        for (var i = 2; i < input.Length; i++)
        {
            nodes.Add(new Node { Name = input[i].Split('=')[0].Trim() });
        }

        for (var i = 2; i < input.Length; i++)
        {
            var node = nodes.FirstOrDefault(x => x.Name == input[i].Split('=')[0].Trim())!;
            var others = input[i].Replace("(", "").Replace(")", "").Trim().Split('=')[1].Split(",");
            node.Left = nodes.FirstOrDefault(x => x.Name == others[0].Trim())!;
            node.Right = nodes.FirstOrDefault(x => x.Name == others[1].Trim())!;
        }

        var currentNodes = nodes.Where(x => x.Name.EndsWith('A')).ToList();

        var stepsList = new List<long>();
        foreach (var c in currentNodes)
        {
            var currentNode = c;
            long steps = 0;
            var index = 0;
            while (!currentNode.Name.EndsWith('Z'))
            {
                currentNode = directions[index] == 'R' ? currentNode.Right : currentNode.Left;
                index++;
                index %= directions.Length;
                steps++;
            }
            stepsList.Add(steps);
        }

        var val = stepsList[0];

        for (var i = 1; i < stepsList.Count; i++)
        {
            val = Lcm(val, stepsList[i]);
        }

        return val.ToString();
    }

    private class Node
    {
        public string Name { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    // Greatest common divisor
    private long Gcd(long a, long b)
    {
        while (a != b)
        {
            if (a > b)
            {
                a -= b;
            }
            else
            {
                b -= a;
            }
        }

        return a;
    }

    // LEast common multiple
    private long Lcm(long a, long b)
    {
        return Math.Abs(a * b) / Gcd(a, b);
    }
}
