using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2020.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        var parsedData = this.InputToLines()
            .Select(Parse)
            .ToList();

        var allIngredients = new HashSet<string>(parsedData.SelectMany(x => x.Item1));
        var allAllergens = new HashSet<string>(parsedData.SelectMany(x => x.Item2));

        var uniqueAllergens = allAllergens.ToDictionary(
            allergen => allergen,
            allergen => new HashSet<string>(parsedData
                .Where(x => x.Item2.Contains(allergen))
                .Select(x => x.Item1)
                .Aggregate((a, b) => a.Intersect(b).ToHashSet()))
        );

        var freeAllergens = new List<(string, string)>();
        while (uniqueAllergens.Count > 0)
        {
            var (allergen, ingredients) = uniqueAllergens.First(x => x.Value.Count == 1);
            var ingredient = ingredients.First();
            freeAllergens.Add((allergen, ingredient));
            uniqueAllergens.Remove(allergen);

            foreach (var value in uniqueAllergens.Values)
            {
                value.Remove(ingredient);
            }
        }

        var freeIngredients = freeAllergens
            .OrderBy(x => x.Item1)
            .Select(x => x.Item2)
            .ToArray();

        var dangerousIngredients = allIngredients
            .Except(freeIngredients)
            .ToList();

        return parsedData
            .Sum(x => 
                x.Item1.Count(ingredient =>
                    dangerousIngredients.Contains(ingredient)))
            .ToString();
    }

    public string SolveSecond()
    {
        var parsedData = this.InputToLines()
            .Select(Parse)
            .ToList();

        var allAllergens = new HashSet<string>(parsedData.SelectMany(x => x.Item2));

        var uniqueAllergens = allAllergens.ToDictionary(
            allergen => allergen,
            allergen => new HashSet<string>(parsedData
                .Where(x => x.Item2.Contains(allergen))
                .Select(x => x.Item1)
                .Aggregate((a, b) => a.Intersect(b).ToHashSet()))
        );

        var freeAllergens = new List<(string, string)>();
        while (uniqueAllergens.Count > 0)
        {
            var (allergen, ingredients) = uniqueAllergens.First(x => x.Value.Count == 1);
            var ingredient = ingredients.First();
            freeAllergens.Add((allergen, ingredient));
            uniqueAllergens.Remove(allergen);

            foreach (var value in uniqueAllergens.Values)
            {
                value.Remove(ingredient);
            }
        }

        var freeIngredients = freeAllergens
            .OrderBy(x => x.Item1)
            .Select(x => x.Item2)
            .ToArray();

        return string.Join(",", freeIngredients);
    }

    private static (HashSet<string>, HashSet<string>) Parse(string line)
    {
        var parts = line[..^1].Split(" (contains");
        var ingredients = new HashSet<string>(parts[0].Split());
        var allergens = new HashSet<string>(parts[1].Replace(" ", "").Split(','));
        return (ingredients, allergens);
    }
}