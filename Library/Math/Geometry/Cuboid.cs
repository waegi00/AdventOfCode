using System.Numerics;

namespace AdventOfCode.Library.Math.Geometry;

/// <summary>
/// Generic class for cuboids 
/// </summary>
/// <typeparam name="T">number type</typeparam>
/// <param name="Length"></param>
/// <param name="Width"></param>
/// <param name="Height"></param>
public record Cuboid<T>(T Length, T Width, T Height) where T : INumber<T>
{
    /// <summary>
    /// Calculates the surface area of the cuboid
    /// </summary>
    /// <returns>2 * (length * width + length * height + width * height)</returns>
    public T Surface()
    {
        dynamic l = Length, w = Width, h = Height;
        return 2 * (l * w + l * h + w * h);
    }

    /// <summary>
    /// Calculates the area of each side
    /// </summary>
    /// <returns>(length * width, length * height, width * height)</returns>
    public (T, T, T) Sides()
    {
        return (Length * Width, Length * Height, Width * Height);
    }
}