namespace AdventOfCode.Library.Array;

public static class ArrayHelper
{
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
}