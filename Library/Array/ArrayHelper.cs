namespace AdventOfCode.Library.Array;

public static class ArrayHelper
{
    /// <summary>
    /// Prints the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">2-D array</param>
    public static void Print<T>(this T[][] array)
    {
        foreach (var row in array)
        {
            foreach (var cell in row)
            {
                Console.Write(cell);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Returns whether a position is valid in a given array or not
    /// </summary>
    /// <param name="array">2-D array</param>
    /// <param name="i">x-position</param>
    /// <param name="j">y-position</param>
    /// <returns>true if i,j is a valid position within array</returns>
    public static bool IsValidPosition<T>(this T[][] array, int i, int j)
    {
        return i >= 0 && i < array.Length && j >= 0 && j < array[i].Length;
    }

    /// <summary>
    /// Returns all neighbouring cell values and indices
    /// </summary>
    /// <param name="array">2-D Array in which to find neighbours</param>
    /// <param name="i">x-position of cell</param>
    /// <param name="j">y-position of cell</param>
    /// <param name="includeHorizontal">whether to include horizontal neighbours or not</param>
    /// <param name="includeVertical">whether to include vertical neighbours or not</param>
    /// <param name="includeDiagonal">whether to include diagonal neighbours or not</param>
    /// <returns>(cell value, (x-pos, y-pos))</returns>
    public static IEnumerable<(T, (int, int))> Neighbours<T>(this T[][] array, int i, int j, bool includeHorizontal = true, bool includeVertical = true, bool includeDiagonal = true)
    {
        if (!IsValidPosition(array, i, j))
        {
            yield break;
        }

        List<(int, int)> directions = [];
        if (includeHorizontal)
        {
            directions.AddRange([(0, -1), (0, 1)]);
        }

        if (includeVertical)
        {
            directions.AddRange([(-1, 0), (1, 0)]);

        }

        if (includeDiagonal)
        {
            directions.AddRange([(-1, -1), (-1, 1), (1, -1), (1, 1)]);

        }

        foreach (var (di, dj) in directions)
        {
            var ni = i + di;
            var nj = j + dj;

            if (!IsValidPosition(array, ni, nj))
            {
                continue;
            }

            yield return (array[ni][nj], (ni, nj));
        }
    }

    /// <summary>
    /// Updates all values in a given range
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">2-D array</param>
    /// <param name="i">start x-position of range</param>
    /// <param name="j">start y-position of range</param>
    /// <param name="i2">end x-position of range</param>
    /// <param name="j2">end y-position of range</param>
    /// <param name="f">Function to apply to the values</param>
    public static void SetValues<T>(this T[][] array, int i, int j, int i2, int j2, Func<T, T> f)
    {
        if (!IsValidPosition(array, i, j) || !IsValidPosition(array, i2, j2)) return;

        for (var row = i; row <= i2; row++)
        {
            for (var col = j; col <= j2; col++)
            {
                array[row][col] = f(array[row][col]);
            }
        }
    }

    /// <summary>
    /// Returns the position of first occurrence or the types default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="target">Value to find</param>
    /// <returns>(x-position, y-position) of first match</returns>
    public static (int i, int j) FindFirst<T>(this T[][] array, T target)
    {
        var query = array
            .SelectMany((row, i) => row.Select((v, j) => new { v, i, j }))
            .First(item => item.v!.Equals(target));

        return (query.i, query.j);
    }


    /// <summary>
    /// Returns the position of all occurrences or the types default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="target">Value to find</param>
    /// <returns>IEnumerable with (x-position, y-position) of the matches</returns>
    public static IEnumerable<(int i, int j)> FindAll<T>(this T[][] array, T target)
    {
        var query = array
            .SelectMany((row, i) => row.Select((v, j) => new { v, i, j }))
            .Where(item => item.v!.Equals(target))
            .Select(item => (item.i, item.j));

        return query;
    }

    /// <summary>
    /// Returns all possible permutations of the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="length">Length to stop recursion</param>
    /// <returns>IEnumerable with all possible permutations</returns>
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> array, int length)
    {
        if (length == 1)
            return array.Select(item => new[] { item });

        var enumerable = array as T[] ?? array.ToArray();
        return GetPermutations(enumerable, length - 1)
            .SelectMany(items => enumerable.Where(item => !items.Contains(item)),
                (items, item) => items.Concat([item]));
    }

    /// <summary>
    /// Generates all possible pairs of an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <returns>IEnumerable with all pairs</returns>
    public static IEnumerable<(T First, T Second)> Pairs<T>(this IEnumerable<T> array)
    {
        var enumerable = array as T[] ?? array.ToArray();

        var pairs = enumerable
            .SelectMany((x, i) => enumerable.Skip(i + 1), (x, y) => (x, y));

        return pairs;
    }
}