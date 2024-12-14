using System.Text.RegularExpressions;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day15 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var ingredients = input.Select(x =>
        {
            var matches = new Regex("-?\\d+").Matches(x);
            var nums = matches.Select(m => int.Parse(m.Value)).ToList();
            return new Ingredient(nums[0], nums[1], nums[2], nums[3], nums[4]);
        }).ToList();

        var results = new List<int[]>();

        GenerateCombinations(100, ingredients.Count, new int[ingredients.Count], 0, results);

        var max = results.Select(combination => Score(combination, ingredients).score).Prepend(0L).Max();

        return max.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var ingredients = input.Select(x =>
        {
            var matches = new Regex("-?\\d+").Matches(x);
            var nums = matches.Select(m => int.Parse(m.Value)).ToList();
            return new Ingredient(nums[0], nums[1], nums[2], nums[3], nums[4]);
        }).ToList();

        var results = new List<int[]>();

        GenerateCombinations(100, ingredients.Count, new int[ingredients.Count], 0, results);

        var max = results
            .Select(combination => Score(combination, ingredients))
            .Where(x => x.calories == 500)
            .Select(x => x.score)
            .Prepend(0L)
            .Max();

        return max.ToString();
    }

    private record Ingredient(int Capacity, int Durability, int Flavor, int Texture, int Calories);

    private static (long score, long calories) Score(int[] amounts, List<Ingredient> ingredients)
    {
        var capacity = 0L;
        var durability = 0L;
        var flavor = 0L;
        var texture = 0L;
        var calories = 0L;

        foreach (var (ingredient, amount) in ingredients.Zip(amounts))
        {
            capacity += ingredient.Capacity * amount;
            durability += ingredient.Durability * amount;
            flavor += ingredient.Flavor * amount;
            texture += ingredient.Texture * amount;
            calories += ingredient.Calories * amount;
        }

        return (Math.Max(capacity, 0) * Math.Max(durability, 0) * Math.Max(flavor, 0) * Math.Max(texture, 0), calories);
    }

    private static void GenerateCombinations(int total, int parts, int[] combination, int index, List<int[]> results)
    {
        if (index == parts - 1)
        {
            combination[index] = total;
            results.Add((int[])combination.Clone());
            return;
        }

        for (var i = 0; i <= total; i++)
        {
            combination[index] = i;
            GenerateCombinations(total - i, parts, combination, index + 1, results);
        }
    }
}