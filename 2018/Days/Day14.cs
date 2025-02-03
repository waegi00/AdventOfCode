using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2018.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var input = int.Parse(this.InputToText());

        var recipes = new List<int> { 3, 7 };
        var elves = new[] { 0, 1 };

        while (recipes.Count < input + 10)
        {
            var newRecipe = elves.Select(e => recipes[e]).Sum();
            if (newRecipe > 9)
            {
                recipes.Add(newRecipe / 10);
            }
            recipes.Add(newRecipe % 10);

            elves = elves.Select(e => (e + recipes[e] + 1) % recipes.Count).ToArray();
        }

        return string.Join(string.Empty, recipes.Skip(input).Take(10));
    }

    public string SolveSecond()
    {
        var input = this.InputToText().ToCharArray().Select(c => c - '0').ToArray();

        var recipes = new List<int> { 3, 7 };
        var elves = new[] { 0, 1 };
        var index = 0;
        var pos = 0;

        while (pos < input.Length)
        {
            var newRecipe = elves.Select(e => recipes[e]).Sum();
            if (newRecipe > 9)
            {
                recipes.Add(newRecipe / 10);
            }
            recipes.Add(newRecipe % 10);

            elves = elves.Select(e => (e + recipes[e] + 1) % recipes.Count).ToArray();

            while (pos < input.Length && index + pos < recipes.Count)
            {
                if (input[pos] == recipes[index + pos])
                {
                    pos++;
                }
                else
                {
                    pos = 0;
                    index++;
                }
            }
        }

        return index.ToString();
    }
}