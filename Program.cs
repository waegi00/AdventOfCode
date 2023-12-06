﻿using AdventOfCode2023.Days;
using AdventOfCode2023.Days.Interfaces;

var riddles = new List<IRiddle>
{
    new Day6(),
    new Day5(),
    new Day4(),
    new Day3(),
    new Day2(),
    new Day1()
};

riddles.ForEach(riddle =>
{
    Console.WriteLine("Riddle " + riddle.GetType().Name);
    Console.WriteLine("Solution 1: " + riddle.SolveFirst());
    Console.WriteLine("Solution 2: " + riddle.SolveSecond());
    Console.WriteLine("");
});