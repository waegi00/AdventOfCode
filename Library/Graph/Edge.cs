namespace AdventOfCode.Library.Graph;

public record Edge<T>(Vertex From, Vertex To, T Weight);