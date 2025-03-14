using System.Reflection;
using AdventOfCode;
using AdventOfCode.Interfaces;

const int year = 2022;
const bool all = false;
const bool dayCreator = false;

if (dayCreator)
{
    DayCreator.Create(year.ToString());
    return;
}

var typesInNamespace = Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.Namespace == $"AdventOfCode._{year}.Days"
                && t is { IsClass: true, IsAbstract: false }
                && t.Name.StartsWith("Day")
                && int.TryParse(t.Name[3..], out _)
                && typeof(IRiddle).IsAssignableFrom(t))
    .OrderByDescending(t => t.Name);

var riddles = typesInNamespace.Select(Activator.CreateInstance).Cast<IRiddle>();

if (!all)
{
    riddles = riddles.Take(1);
}

riddles.ToList().ForEach(riddle =>
{
    Console.WriteLine("Riddle " + riddle.GetType().Name);
    Console.WriteLine("Solution 1: " + riddle.SolveFirst());
    Console.WriteLine("Solution 2: " + riddle.SolveSecond());
    Console.WriteLine("");
});
