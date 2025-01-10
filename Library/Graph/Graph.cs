using System.Collections.Frozen;
using System.Numerics;
using AdventOfCode.Library.Array;

namespace AdventOfCode.Library.Graph;

public class Graph<T> where T : INumber<T>, IMinMaxValue<T>
{
    private readonly HashSet<Vertex> _vertices = [];

    private readonly HashSet<Edge<T>> _edges = [];

    public FrozenSet<Vertex> Vertices => _vertices.ToFrozenSet();

    public FrozenSet<Edge<T>> Edges => _edges.ToFrozenSet();

    /// <summary>
    /// Gets the weight between to vertices
    /// </summary>
    /// <param name="from">Vertex from</param>
    /// <param name="to">Vertex to</param>
    /// <param name="directed">Whether to consider edges as directed or not</param>
    /// <returns>Weight between vertices if it exists, else 0</returns>
    public T GetWeight(Vertex from, Vertex to, bool directed = false)
    {
        var edge = _edges.FirstOrDefault(e => e.From == from && e.To == to || (!directed && e.To == from && e.From == to));
        return edge != null ? edge.Weight : T.Zero;
    }

    /// <summary>
    /// Adds a vertex if it does not exist yet
    /// </summary>
    /// <param name="vertices">Vertices to add</param>
    public void AddVertices(params Vertex[] vertices)
    {
        foreach (var vertex in vertices)
        {
            _vertices.Add(vertex);
        }
    }

    /// <summary>
    /// Adds an edge if it does not exist yet, also adds the edges vertices if they do not exist yet
    /// </summary>
    /// <param name="edges">Edges to add</param>
    public void AddEdges(params Edge<T>[] edges)
    {
        foreach (var edge in edges)
        {
            if (_edges.Add(edge))
            {
                AddVertices(edge.From, edge.To);
            }
        }
    }

    /// <summary>
    /// Calculates the shortest path that visits all vertices using brute force
    /// </summary>
    /// <param name="isConnectedCircle">Whether to connect back to start or not</param>
    /// <returns>Sum of weights of the shortest path</returns>
    public T ShortestPath(bool isConnectedCircle = false)
    {
        var vertices = _vertices.ToList();
        var n = vertices.Count;

        if (n == 0) return T.Zero;

        var permutations = vertices.ToArray().GetPermutations(n).Select(p => p.ToArray());

        return permutations.Select(x => isConnectedCircle ? CalculatePathCostAsConnectedCircle(x) : CalculatePathCost(x)).Aggregate(T.MaxValue, (a, b) => a.CompareTo(b) < 0 ? a : b);
    }

    /// <summary>
    /// Calculates the shortest path that visits all vertices using brute force, starting at vertex 'start'
    /// </summary>
    /// <param name="start">Start vertex</param>
    /// <param name="isConnectedCircle">Whether to connect back to start or not</param>
    /// <returns>Sum of weights of the shortest path</returns>
    public T ShortestPath(Vertex start, bool isConnectedCircle = false)
    {
        var vertices = _vertices.ToList();
        vertices.Remove(start);
        var n = vertices.Count;

        if (n == 0) return T.Zero;

        var permutations = vertices.GetPermutations(n)
            .Select(p => p.Prepend(start).ToArray()).ToArray();

        return permutations.Select(x => isConnectedCircle ? CalculatePathCostAsConnectedCircle(x) : CalculatePathCost(x)).Aggregate(T.MaxValue, (a, b) => a.CompareTo(b) < 0 ? a : b);
    }

    /// <summary>
    /// Calculates the longest path that visits all vertices using brute force
    /// </summary>
    /// <returns>Sum of weights of the longest path</returns>
    public T LongestPath()
    {
        var vertices = _vertices.ToList();
        var n = vertices.Count;

        if (n == 0) return T.Zero;

        var permutations = vertices.ToArray().GetPermutations(n).Select(p => p.ToArray());

        return permutations.Select(CalculatePathCost).Aggregate(T.Zero, (a, b) => a.CompareTo(b) > 0 ? a : b);
    }

    private T CalculatePathCost(Vertex[] path)
    {
        var totalCost = T.Zero;

        for (var i = 0; i < path.Length - 1; i++)
        {
            var weight = GetWeight(path[i], path[i + 1]);
            if (weight == T.Zero) return T.MaxValue;
            totalCost = Add(totalCost, weight);
        }

        return totalCost;
    }

    private T CalculatePathCostAsConnectedCircle(Vertex[] path)
    {
        var weight = GetWeight(path[^1], path[0]);
        var length = CalculatePathCost(path);
        return weight == T.Zero || length == T.MaxValue ? T.MaxValue : length + weight;
    }

    private static T Add(T a, T b)
    {
        dynamic da = a, db = b;
        return (T)(da + db);
    }
}