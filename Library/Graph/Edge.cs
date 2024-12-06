namespace AdventOfCode.Library.Graph;

public record Edge<T>(Vertex from, Vertex to, T weight);