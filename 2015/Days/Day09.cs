using AdventOfCode.Interfaces;
using AdventOfCode.Library.Graph;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2015.Days;

public class Day09 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines();

        var graph = new Graph<int>();

        foreach (var line in input)
        {
            var splits = line.Split(" = ");
            var vs = splits[0].Split(" to ").Select(x => new Vertex(x)).ToList();
            graph.AddEdges(new Edge<int>(vs[0], vs[1], int.Parse(splits[1])));
        }

        return graph.ShortestPath().ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines();

        var graph = new Graph<int>();

        foreach (var line in input)
        {
            var splits = line.Split(" = ");
            var vs = splits[0].Split(" to ").Select(x => new Vertex(x)).ToList();
            graph.AddEdges(new Edge<int>(vs[0], vs[1], int.Parse(splits[1])));
        }

        return graph.LongestPath().ToString();
    }
}