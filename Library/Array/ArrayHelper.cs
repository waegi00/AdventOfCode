using System.Numerics;

namespace AdventOfCode.Library.Array;

public static class ArrayHelper
{
    /// <summary>
    /// Return whether both arrays are equal by their values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="other">The other array</param>
    /// <returns>True if both arrays are equals by their values</returns>
    public static bool EqualValues<T>(this T[][] array, T[][] other) where T : IComparable<T>
    {
        if (array.Length != other.Length)
        {
            return false;
        }

        for (var i = 0; i < array.Length; i++)
        {
            if (array[i].Length != other[i].Length)
            {
                return false;
            }

            for (var j = 0; j < array[i].Length; j++)
            {
                if (array[i][j].CompareTo(other[i][j]) != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }


    /// <summary>
    /// Prints the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">2-D array</param>
    public static void Print<T>(this IEnumerable<IEnumerable<T>> array)
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
    /// <param name="includeInvalid">whether to include invalid positions</param>
    /// <returns>(cell value, (x-pos, y-pos))</returns>
    public static IEnumerable<(T?, (int, int))> Neighbours<T>(this T[][] array, int i, int j, bool includeHorizontal = true, bool includeVertical = true, bool includeDiagonal = true, bool includeInvalid = false)
    {
        if (!includeInvalid && !IsValidPosition(array, i, j))
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

            if (IsValidPosition(array, ni, nj))
            {
                yield return (array[ni][nj], (ni, nj));
            }
            else if (includeInvalid)
            {
                yield return (default, (ni, nj));
            }
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


    /// <summary>
    /// Generates all subsets of an array, including itself
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <returns>IEnumerable of all subsets</returns>
    public static IEnumerable<IEnumerable<T>> Subsets<T>(this IEnumerable<T> array)
    {
        var enumerable = array as T[] ?? array.ToArray();

        var subsetCount = (int)System.Math.Pow(2, enumerable.Length);

        for (var i = 0; i < subsetCount; i++)
        {
            var subset = enumerable.Where((_, j) => (i & (1 << j)) != 0);

            yield return subset;
        }
    }

    /// <summary>
    /// Returns the product of the numbers in the array
    /// </summary>
    /// <param name="numbers">The array</param>
    /// <returns>Product of all numbers in the array</returns>
    public static T Product<T>(this IEnumerable<T> numbers) where T : INumber<T>
    {
        return numbers.Aggregate(T.One, (product, num) => product * num);
    }

    /// <summary>
    /// Flips all rows in the array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <returns>Flipped array</returns>
    public static T[][] FlipRows<T>(this T[][] array)
    {
        var res = array.Select(x => x.ToArray()).ToArray();

        foreach (var row in res)
        {
            System.Array.Reverse(row);
        }

        return res;
    }

    /// <summary>
    /// Rotates an array 90 degrees to the right
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <returns>Rotated array</returns>
    public static T[][] Rotate90DegreesRight<T>(this T[][] array)
    {
        var n = array.Length;
        var m = array[0].Length;

        var rotatedMatrix = new T[m][];

        for (var i = 0; i < m; i++)
        {
            rotatedMatrix[i] = new T[n];
            for (var j = 0; j < n; j++)
            {
                rotatedMatrix[i][j] = array[n - 1 - j][i];
            }
        }
        return rotatedMatrix;
    }

    /// <summary>
    /// Rotates by n elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="n">amount of cells to rotate</param>
    /// <returns>The rotated array</returns>
    public static T[] Rotate<T>(this T[] array, int n)
    {
        if (n == 0)
        {
            return array;
        }

        n %= array.Length;

        var a = array[..^n].ToList();
        return array[^n..].Concat(a).ToArray();
    }

    /// <summary>
    /// Rotates a row by n elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="row">row index</param>
    /// <param name="n">amount of cells to rotate</param>
    public static void RotateRow<T>(this T[][] array, int row, int n)
    {
        if (n == 0 || !array.IsValidPosition(row, 0))
        {
            return;
        }

        n %= array[row].Length;

        var a = array[row][..^n].ToList();
        var wrap = array[row][^n..].Concat(a).ToArray();
        array[row] = wrap;
    }

    /// <summary>
    /// Rotates a col by n elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array</param>
    /// <param name="col">col index</param>
    /// <param name="n">amount of cells to rotate</param>
    public static void RotateCol<T>(this T[][] array, int col, int n)
    {
        if (n == 0 || Enumerable.Range(0, array.Length).Any(x => !array.IsValidPosition(x, col)))
        {
            return;
        }

        n %= array.Length;

        var a = array[..^n].Select(r => r[col]);
        var wrap = array[^n..].Select(r => r[col]);
        var newCol = wrap.Concat(a).ToArray();
        for (var i = 0; i < array.Length; i++)
        {
            array[i][col] = newCol[i];
        }
    }

    /// <summary>
    /// Finds the index of the max value in the array
    /// </summary>
    /// <param name="array">The array</param>
    /// <returns>Returns index of maximal value (smallest index if multiple maximas)</returns>
    public static int MaxIndex<T>(this T[] array) where T : INumber<T>, IMinMaxValue<T>
    {
        var max = T.MinValue;
        var index = -1;

        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] <= max) continue;

            max = array[i];
            index = i;
        }

        return index;
    }
}