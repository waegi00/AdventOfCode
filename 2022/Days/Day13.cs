using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Text.Json.Nodes;
using AdventOfCode.Library.Array;

namespace AdventOfCode._2022.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        return GetPackets(this.InputToText())
            .Chunk(2)
            .WithIndex()
            .Where(x => Compare(x.item[0], x.item[1]) < 0)
            .Sum(x => x.index + 1)
            .ToString();
    }

    public string SolveSecond()
    {
        var divider = GetPackets("[[2]]\r\n[[6]]").ToList();
        var packets = GetPackets(this.InputToText())
            .Concat(divider)
            .ToList();
        packets.Sort(Compare);

        return ((packets.IndexOf(divider[0]) + 1) * (packets.IndexOf(divider[1]) + 1)).ToString();
    }

    private static IEnumerable<JsonNode> GetPackets(string input) =>
        input.Split("\r\n")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => JsonNode.Parse(x)!);

    private static int Compare(JsonNode nodeA, JsonNode nodeB)
    {
        if (nodeA is JsonValue && nodeB is JsonValue)
        {
            return (int)nodeA - (int)nodeB;
        }

        var arrayA = nodeA as JsonArray ?? new JsonArray((int)nodeA);
        var arrayB = nodeB as JsonArray ?? new JsonArray((int)nodeB);
        return arrayA.Zip(arrayB)
            .Select(x => Compare(x.First!, x.Second!))
            .FirstOrDefault(c => c != 0, arrayA.Count - arrayB.Count);
    }
}