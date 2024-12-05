using AdventOfCode.Interfaces;

namespace AdventOfCode._2023.Days;

public class Day02 : IRiddle
{
    public string SolveFirst()
    {
        var games = CreateGames();

        return games.Where(x => x.CanHappen).Sum(x => x.Id).ToString();
    }

    public string SolveSecond()
    {
        var games = CreateGames();

        return games.Sum(x => x.MaxColorMultiplied).ToString();
    }

    private class Game
    {
        public int Id { get; set; }
        public bool CanHappen => Sets.All(s => s is { Red: <= 12, Green: <= 13, Blue: <= 14 });
        public int MaxColorMultiplied => Sets.Max(s => s.Red) * Sets.Max(s => s.Blue) * Sets.Max(s => s.Green);
        public List<Set> Sets { get; set; } = new List<Set>();
    }

    private class Set
    {
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
    }

    private static IEnumerable<Game> CreateGames()
    {
        var input = File.ReadAllLines("2023\\Days\\Inputs\\Day2.txt");

        var games = new List<Game>();

        foreach (var line in input)
        {
            var splits = line.Split(':');
            var game = new Game { Id = int.Parse(splits[0].Split(" ")[1]) };
            foreach (var s in splits[1].Split(";"))
            {
                int red = 0, blue = 0, green = 0;
                var colors = s.Split(",");
                foreach (var color in colors)
                {
                    var colNum = color.Trim().Split(" ");
                    switch (colNum[1].Trim())
                    {
                        case "red":
                            red = int.Parse(colNum[0].Trim());
                            break;
                        case "blue":
                            blue = int.Parse(colNum[0].Trim());
                            break;
                        case "green":
                            green = int.Parse(colNum[0].Trim());
                            break;
                    }
                }

                game.Sets.Add(new Set() { Red = red, Blue = blue, Green = green });
            }

            games.Add(game);
        }

        return games;
    }
}
