namespace AdventOfCode.Library.Char;

public class JaggedArrayComparer<T>(IEqualityComparer<T>? elementComparer = null) : IEqualityComparer<T[][]>
{
    private readonly IEqualityComparer<T> _elementComparer = elementComparer ?? EqualityComparer<T>.Default;

    public bool Equals(T[][]? x, T[][]? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        return !x.Where((t, i) => !t.SequenceEqual(y[i], _elementComparer)).Any();
    }

    public int GetHashCode(T[][]? obj)
    {
        if (obj == null)
        {
            return 0;
        }

        unchecked
        {
            return obj.Aggregate(17, (current1, subArray) =>
                subArray.Aggregate(current1, (current, item) => 
                    current * 31 + (_elementComparer.GetHashCode(item))));
        }
    }
}