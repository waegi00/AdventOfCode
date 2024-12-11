using System.Xml;
using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day16 : IRiddle
{
    private readonly Sue givenSue = new(-1, 3, 7, 2, 3, 0, 0, 5, 3, 2, 1);

    public string SolveFirst()
    {
        var input = this.InputToLines();

        var sues = input.Select((x, i) => ParseSue(x, i + 1));

        foreach (var sue in sues)
        {
            if (sue.children != -1 && sue.children != givenSue.children) continue;
            if (sue.cats != -1 && sue.cats != givenSue.cats) continue;
            if (sue.samoyeds != -1 && sue.samoyeds != givenSue.samoyeds) continue;
            if (sue.pomeranians != -1 && sue.pomeranians != givenSue.pomeranians) continue;
            if (sue.akitas != -1 && sue.akitas != givenSue.akitas) continue;
            if (sue.vizslas != -1 && sue.vizslas != givenSue.vizslas) continue;
            if (sue.goldfish != -1 && sue.goldfish != givenSue.goldfish) continue;
            if (sue.trees != -1 && sue.trees != givenSue.trees) continue;
            if (sue.cars != -1 && sue.cars != givenSue.cars) continue;
            if (sue.perfumes != -1 && sue.perfumes != givenSue.perfumes) continue;
            return sue.number.ToString();
        }

        return "";
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var sues = input.Select((x, i) => ParseSue(x, i + 1));

        foreach (var sue in sues)
        {
            if (sue.children != -1 && sue.children != givenSue.children) continue;
            if (sue.cats != -1 && sue.cats <= givenSue.cats) continue;
            if (sue.samoyeds != -1 && sue.samoyeds != givenSue.samoyeds) continue;
            if (sue.pomeranians != -1 && sue.pomeranians > givenSue.pomeranians) continue;
            if (sue.akitas != -1 && sue.akitas != givenSue.akitas) continue;
            if (sue.vizslas != -1 && sue.vizslas != givenSue.vizslas) continue;
            if (sue.goldfish != -1 && sue.goldfish > givenSue.goldfish) continue;
            if (sue.trees != -1 && sue.trees <= givenSue.trees) continue;
            if (sue.cars != -1 && sue.cars != givenSue.cars) continue;
            if (sue.perfumes != -1 && sue.perfumes != givenSue.perfumes) continue;
            return sue.number.ToString();
        }

        return "";
    }

    private record Sue(int number, int children, int cats, int samoyeds, int pomeranians, int akitas, int vizslas, int goldfish, int trees, int cars, int perfumes);

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