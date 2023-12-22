using AdventOfCode2023.Days;
using AdventOfCode2023.Days.Interfaces;

var riddles = new List<IRiddle>
{
    new Day17(),
    //new Day16(), // Takes to long
    //new Day15(),
    //new Day14(),
    //new Day13(),
    //new Day12(),
    //new Day11(),
    //new Day10(),
    //new Day9(),
    //new Day8(),
    //new Day7(),
    //new Day6(),
    //new Day5(),
    //new Day4(),
    //new Day3(),
    //new Day2(),
    //new Day1()
};

riddles.ForEach(riddle =>
{
    Console.WriteLine("Riddle " + riddle.GetType().Name);
    Console.WriteLine("Solution 1: " + riddle.SolveFirst());
    Console.WriteLine("Solution 2: " + riddle.SolveSecond());
    Console.WriteLine("");
});
