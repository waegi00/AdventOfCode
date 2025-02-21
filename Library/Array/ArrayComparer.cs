namespace AdventOfCode.Library.Array;

public class ArrayComparer<T>(IEqualityComparer<T>? elementComparer = null) : IEqualityComparer<T[]>
{
    private readonly IEqualityComparer<T> _elementComparer = elementComparer ?? EqualityComparer<T>.Default;

    public bool Equals(T[]? x, T[]? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Length == y.Length && x.SequenceEqual(y, _elementComparer);
    }

    public int GetHashCode(T[]? obj)
    {
        if (obj == null)
        {
            return 0;
        }

        unchecked
        {
            return obj.Aggregate(17, (current, item) =>
                current * 31 + _elementComparer.GetHashCode(item!));
        }
    }
}
