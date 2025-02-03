namespace AdventOfCode;

public static class DayCreator
{
    public static void Create(string year)
    {
        const string appPath = @"C:\__code\AdventOfCode";
        var basePath = Path.Combine(appPath, year, "Days");
        Directory.CreateDirectory(basePath);

        var existingDays = Directory.GetFiles(basePath, "Day*.cs")
            .Select(f => Path.GetFileNameWithoutExtension(f).Replace("Day", ""))
            .Where(d => int.TryParse(d, out _))
            .Select(int.Parse)
            .OrderBy(n => n)
            .ToList();

        var nextDay = existingDays.Count > 0 ? existingDays.Max() + 1 : 1;
        var day = nextDay.ToString("D2");

        var inputFilePath = Path.Combine(basePath, "Inputs", $"Day{day}.txt");
        var classFilePath = Path.Combine(basePath, $"Day{day}.cs");

        Directory.CreateDirectory(Path.GetDirectoryName(inputFilePath) ?? string.Empty);

        File.WriteAllText(inputFilePath, "");

        var classContent = $$"""
                             using AdventOfCode.Interfaces;

                             namespace AdventOfCode._{{year}}.Days;

                             public class Day{{day}} : IRiddle
                             {
                                 public string SolveFirst()
                                 {
                                     return "";
                                 }
                             
                                 public string SolveSecond()
                                 {
                                     return "";
                                 }
                             }
                             """;

        File.WriteAllText(classFilePath, classContent);

        Console.WriteLine($"Files created successfully:\n{inputFilePath}\n{classFilePath}");
    }
}