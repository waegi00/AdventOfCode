using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day16 : IRiddle
{
    private readonly Sue _givenSue = new(-1, 3, 7, 2, 3, 0, 0, 5, 3, 2, 1);

    public string SolveFirst()
    {
        var input = this.InputToLines();

        var sues = input.Select((x, i) => ParseSue(x, i + 1));

        foreach (var sue in sues)
        {
            if (sue.Children != -1 && sue.Children != _givenSue.Children) continue;
            if (sue.Cats != -1 && sue.Cats != _givenSue.Cats) continue;
            if (sue.Samoyeds != -1 && sue.Samoyeds != _givenSue.Samoyeds) continue;
            if (sue.Pomeranians != -1 && sue.Pomeranians != _givenSue.Pomeranians) continue;
            if (sue.Akitas != -1 && sue.Akitas != _givenSue.Akitas) continue;
            if (sue.Vizslas != -1 && sue.Vizslas != _givenSue.Vizslas) continue;
            if (sue.Goldfish != -1 && sue.Goldfish != _givenSue.Goldfish) continue;
            if (sue.Trees != -1 && sue.Trees != _givenSue.Trees) continue;
            if (sue.Cars != -1 && sue.Cars != _givenSue.Cars) continue;
            if (sue.Perfumes != -1 && sue.Perfumes != _givenSue.Perfumes) continue;
            return sue.Number.ToString();
        }

        return "";
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var sues = input.Select((x, i) => ParseSue(x, i + 1));

        foreach (var sue in sues)
        {
            if (sue.Children != -1 && sue.Children != _givenSue.Children) continue;
            if (sue.Cats != -1 && sue.Cats <= _givenSue.Cats) continue;
            if (sue.Samoyeds != -1 && sue.Samoyeds != _givenSue.Samoyeds) continue;
            if (sue.Pomeranians != -1 && sue.Pomeranians > _givenSue.Pomeranians) continue;
            if (sue.Akitas != -1 && sue.Akitas != _givenSue.Akitas) continue;
            if (sue.Vizslas != -1 && sue.Vizslas != _givenSue.Vizslas) continue;
            if (sue.Goldfish != -1 && sue.Goldfish > _givenSue.Goldfish) continue;
            if (sue.Trees != -1 && sue.Trees <= _givenSue.Trees) continue;
            if (sue.Cars != -1 && sue.Cars != _givenSue.Cars) continue;
            if (sue.Perfumes != -1 && sue.Perfumes != _givenSue.Perfumes) continue;
            return sue.Number.ToString();
        }

        return "";
    }

    private record Sue(int Number, int Children, int Cats, int Samoyeds, int Pomeranians, int Akitas, int Vizslas, int Goldfish, int Trees, int Cars, int Perfumes);

    private static Sue ParseSue(string str, int number)
    {
        var children = -1;
        var cats = -1;
        var samoyeds = -1;
        var pomeranians = -1;
        var akitas = -1;
        var vizslas = -1;
        var goldfish = -1;
        var trees = -1;
        var cars = -1;
        var perfumes = -1;

        var splits = str.Replace(",", "").Split(" ")[2..];
        for (var i = 0; i < splits.Length; i += 2)
        {
            var amount = int.Parse(splits[i + 1]);
            switch (splits[i][..^1])
            {
                case "children":
                    children = amount;
                    break;
                case "cats":
                    cats = amount;
                    break;
                case "samoyeds":
                    samoyeds = amount;
                    break;
                case "pomeranians":
                    pomeranians = amount;
                    break;
                case "akitas":
                    akitas = amount;
                    break;
                case "vizslas":
                    vizslas = amount;
                    break;
                case "goldfish":
                    goldfish = amount;
                    break;
                case "trees":
                    trees = amount;
                    break;
                case "cars":
                    cars = amount;
                    break;
                case "perfumes":
                    perfumes += amount;
                    break;
            }
        }

        return new Sue(number, children, cats, samoyeds, pomeranians, akitas, vizslas, goldfish, trees, cars, perfumes);
    }
}