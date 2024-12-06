using System.Numerics;

namespace AdventOfCode.Library.Math.Geometry;

/// <summary>
/// Generic class for cuboids 
/// </summary>
/// <typeparam name="T">number type</typeparam>
/// <param name="length"></param>
/// <param name="width"></param>
/// <param name="height"></param>
public record Cuboid<T>(T length, T width, T height) where T : INumber<T>
{
    /// <summary>
    /// Calculates the surface area of the cuboid
    /// </summary>
    /// <returns>2 * (length * width + length * height + width * height)</returns>
    public T Surface()
    {
        dynamic l = length, w = width, h = height;
        return 2 * (l * w + l * h + w * h);
    }

    /// <summary>
    /// Calculates the area of each side
    /// </summary>
    /// <returns>(length * width, length * height, width * height)</returns>
    public (T, T, T) Sides()
    {
        return (length * width, length * height, width * height);
    }
}