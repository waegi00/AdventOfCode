namespace AdventOfCode.Library.Array
{
    public static class IEnumerableHelper
    {
        /// <summary>
        /// Returns the enumerable with its indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>A modified enumerable containing the index of each item</returns>
        public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((item, index) => (item, index));
        }
    }
}
