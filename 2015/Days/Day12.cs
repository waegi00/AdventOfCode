using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace AdventOfCode._2015.Days;

public partial class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToText();

        var sum = 0;

        foreach (Match match in MyRegex().Matches(input))
        {
            sum += int.Parse(match.Value);
        }

        return sum.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToText();

        var json = JsonSerializer.Deserialize<JsonElement>(input);

        return CalculateSumExcludingRed(json).ToString();
    }

    private static int CalculateSumExcludingRed(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Number:
                return element.GetInt32();
            case JsonValueKind.Array:
            {
                return element.EnumerateArray().Sum(CalculateSumExcludingRed);
            }
            case JsonValueKind.Object:
            {
                var isRed = element.EnumerateObject().Any(property => property.Value.ValueKind == JsonValueKind.String && property.Value.GetString() == "red");
                return isRed ? 0 : element.EnumerateObject().Sum(property => CalculateSumExcludingRed(property.Value));
            }
            case JsonValueKind.Undefined:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                return 0;
        }
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex MyRegex();
}