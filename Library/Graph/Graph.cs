using System.Collections.Frozen;
using System.Numerics;

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
        var edge = _edges.FirstOrDefault(e => e.from == from && e.to == to || (!directed && e.to == from && e.from == to));
        return edge != null ? edge.weight : T.Zero;
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
                AddVertices(edge.from, edge.to);
            }
        }
    }

    /// <summary>
    /// Calculates the shortest path that visits all vertices using brute force
    /// </summary>
    /// <returns>Sum of weights of the shortest path</returns>
    public T ShortestPath()
    {
        var vertices = _vertices.ToList();
        var n = vertices.Count;

        if (n == 0) return T.Zero;

        var permutations = GetPermutations(vertices, n);

        return permutations.Select(CalculatePathCost).Aggregate(T.MaxValue, (a, b) => a.CompareTo(b) < 0 ? a : b);
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

        var permutations = GetPermutations(vertices, n);

        return permutations.Select(CalculatePathCost).Aggregate(T.Zero, (a, b) => a.CompareTo(b) > 0 ? a : b);
    }

    private static IEnumerable<List<Vertex>> GetPermutations(List<Vertex> vertices, int length)
    {
        if (length == 1)
            return vertices.Select(t => new List<Vertex> { t });

        return GetPermutations(vertices, length - 1)
            .SelectMany(t => vertices.Where(e => !t.Contains(e)),
                (t, e) => t.Concat(new[] { e }).ToList());
    }

    private T CalculatePathCost(List<Vertex> path)
    {
        var totalCost = T.Zero;

        for (var i = 0; i < path.Count - 1; i++)
        {
            var weight = GetWeight(path[i], path[i + 1]);
            if (weight == T.Zero) return T.MaxValue;
            totalCost = Add(totalCost, weight);
        }

        return totalCost;
    }

    private static T Add(T a, T b)
    {
        dynamic da = a, db = b;
        return (T)(da + db);
    }
}