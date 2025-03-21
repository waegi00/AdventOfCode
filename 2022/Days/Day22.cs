using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.Math.Numbers;
using AdventOfCode.Library.String;

namespace AdventOfCode._2022.Days;

public class Day22 : IRiddle
{
    private static readonly Dictionary<int, int> imnj = new();
    private static readonly Dictionary<int, int> jmni = new();
    private static readonly Dictionary<int, int> imxj = new();
    private static readonly Dictionary<int, int> jmxi = new();
    private static readonly Dictionary<(int, int), (Vector3, Vector3, Vector3)> faces = new();
    private static readonly Dictionary<(Vector3, Vector3), (int, int)> edges = new();

    public string SolveFirst()
    {
        var input = this.InputToLines();

        var grid = input[..^2].ToCharArray();
        var route = input[^1]
            .Replace("R", ",R,")
            .Replace("L", ",L,")
            .Split(',');

        var (x, y) = (0, input[0].IndexOf('.'));
        var (dx, dy) = (0, 1);

        foreach (var r in route)
        {
            if (int.TryParse(r, out var n))
            {
                var canMove = true;
                while (n > 0 && canMove)
                {
                    var nx = (x + dx).Mod(grid.Length);
                    var ny = (y + dy).Mod(grid[x].Length);

                    while (!grid.IsValidPosition(nx, ny) || grid[nx][ny] == ' ')
                    {
                        nx = (nx + dx).Mod(grid.Length);
                        ny = (ny + dy).Mod(grid[x].Length);
                    }

                    switch (grid[nx][ny])
                    {
                        case '#':
                            canMove = false;
                            break;
                        case '.':
                            x = nx;
                            y = ny;
                            n--;
                            break;
                    }
                }
            }
            else
            {
                (dx, dy) = r switch
                {
                    "R" => (dy, -dx),
                    "L" => (-dy, dx),
                    _ => (dx, dy)
                };
            }
        }

        return (1000 * (x + 1) + 4 * (y + 1) + Score(dx, dy)).ToString();
    }

    public string SolveSecond()
    {
        var parts = this.InputToText().Split("\r\n\r\n");
        var grid = parts[0].Split("\r\n");
        var b = (int)Math.Sqrt(grid.Sum(row => (double)row.Count(c => c is '#' or '.')) / 6);

        foreach (var x in Enumerable.Range(0, grid.Length))
        {
            foreach (var y in Enumerable.Range(0, grid[x].Length))
            {
                if (grid[x][y] == ' ') continue;
                imnj.TryAdd(x, int.MaxValue);
                imxj.TryAdd(x, int.MinValue);
                jmni.TryAdd(y, int.MaxValue);
                jmxi.TryAdd(y, int.MinValue);

                imnj[x] = Math.Min(imnj[x], y);
                imxj[x] = Math.Max(imxj[x], y);
                jmni[y] = Math.Min(jmni[y], x);
                jmxi[y] = Math.Max(jmxi[y], x);
            }
        }

        var j0 = Enumerable.Range(0, grid[0].Length).First(j => grid[0][j] == '.');
        Move(grid, 0, j0, new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), b);
        int i = 0, j = Enumerable.Range(0, grid[0].Length).First(j => grid[0][j] == '.');
        int di = 0, dj = 1;

        foreach (var cmd in parts[1].Replace("R", ",R,").Replace("L", ",L,").Split(','))
        {
            switch (cmd)
            {
                case "L":
                    (di, dj) = (-dj, di);
                    break;
                case "R":
                    (di, dj) = (dj, -di);
                    break;
                default:
                    (i, j, di, dj) = Step(grid, int.Parse(cmd), i, j, di, dj, b);
                    break;
            }
        }

        return (1000 * (i + 1) + 4 * (j + 1) + Score(di, dj)).ToString();
    }

    private static int Score(int dx, int dy)
    {
        return (dx, dy) switch
        {
            (0, 1) => 0,
            (1, 0) => 1,
            (0, -1) => 2,
            (-1, 0) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static bool InBounds(string[] grid, int i, int j)
    {
        return i >= 0 && i < grid.Length && j >= 0 && j < grid[i].Length && grid[i][j] != ' ';
    }

    private static void Move(string[] grid, int i, int j, Vector3 xyz, Vector3 di, Vector3 dj, int b)
    {
        while (true)
        {
            if (!InBounds(grid, i, j) || faces.ContainsKey((i, j))) return;

            faces[(i, j)] = (xyz, di, dj);

            for (var r = 0; r < b; r++)
            {
                edges[(xyz + di * r, di.Cross(dj))] = (i + r, j);
                edges[(xyz + di * r + dj * (b - 1), di.Cross(dj))] = (i + r, j + b - 1);
                edges[(xyz + dj * r, di.Cross(dj))] = (i, j + r);
                edges[(xyz + dj * r + di * (b - 1), di.Cross(dj))] = (i + b - 1, j + r);
            }

            Move(grid, i + b, j, xyz + di * (b - 1), di.Cross(dj), dj, b);
            Move(grid, i - b, j, xyz + di.Cross(dj) * (b - 1), dj.Cross(di), dj, b);
            Move(grid, i, j + b, xyz + dj * (b - 1), di, di.Cross(dj), b);
            j -= b;
            xyz += di.Cross(dj) * (b - 1);
            dj = dj.Cross(di);
        }
    }

    private static (int, int, int, int) Step(string[] grid, int x, int i, int j, int di, int dj, int b)
    {
        for (var k = 0; k < x; k++)
        {
            int ii = i + di, jj = j + dj, ddi = di, ddj = dj;
            if (!InBounds(grid, ii, jj))
            {
                var (xyz, di3, dj3) = faces[(i / b * b, j / b * b)];
                var here = xyz + di3 * (i % b) + dj3 * (j % b);
                var n = di3.Cross(dj3);
                (ii, jj) = edges[(here, di3 * -di + dj3 * -dj)];
                (_, di3, dj3) = faces[(ii / b * b, jj / b * b)];
                ddi = di3.Dot(n);
                ddj = dj3.Dot(n);
            }
            if (grid[ii][jj] == '#') break;
            i = ii; j = jj; di = ddi; dj = ddj;
        }
        return (i, j, di, dj);
    }

    private record Vector3(int x, int y, int z)
    {
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator *(Vector3 a, int k) => new(a.x * k, a.y * k, a.z * k);
        public Vector3 Cross(Vector3 other) => new(y * other.z - z * other.y, z * other.x - x * other.z, x * other.y - y * other.x);
        public int Dot(Vector3 other) => x * other.x + y * other.y + z * other.z;
    }
}
