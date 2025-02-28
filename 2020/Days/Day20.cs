using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2020.Days;

public class Day20 : IRiddle
{
    private readonly char[][] monster = """
                                        ..................#.
                                        #....##....##....###
                                        .#..#..#..#..#..#...
                                        """.Split("\r\n").ToCharArray();
    public string SolveFirst()
    {
        return Solve(ParseInput(this.InputToText()), true)
            .ToString();
    }

    public string SolveSecond()
    {
        return Solve(ParseInput(this.InputToText()), false)
            .ToString();
    }

    private static string[][] ParseInput(string input)
    {
        return input.Split("\r\n\r\n").Select(x => x.Split("\r\n").ToArray()).ToArray();
    }

    private long Solve(string[][] input, bool isPart1)
    {
        var tiles = input.Select(g => new Tile(g)).ToArray();

        var gridSize = (int)Math.Sqrt(tiles.Length);
        var corners = tiles.Where(t => t.Matches(tiles) == 2).ToList();
        if (isPart1)
        {
            return corners.Aggregate(1L, (i, v) => i * v.ID);
        }

        var sides = new Dictionary<int, List<Tile>>(
            tiles.SelectMany(t => t.Sides.Select(s => new { s, t }))
            .GroupBy(s => s.s)
            .Where(s => s.Count() > 1)
            .Select(s => new KeyValuePair<int, List<Tile>>(s.Key, s.Select(i => i.t).ToList()))
        );

        var assembled = new Tile[gridSize, gridSize];
        for (var c = 0; c < gridSize; c++)
        {
            for (var r = 0; r < gridSize; r++)
            {
                Tile nextTile;
                if (c == 0 && r == 0)
                {
                    nextTile = corners.First();
                    while (!sides.ContainsKey(nextTile.GetSide(Tile.SideName.Right)) ||
                           !sides.ContainsKey(nextTile.GetSide(Tile.SideName.Bottom)))
                    {
                        nextTile.Rotate();
                    }
                }
                else if (r == 0)
                {
                    var lastRight = assembled[r, c - 1].GetSide(Tile.SideName.Right);
                    nextTile = sides[lastRight].Single(e => !e.IsPlaced);
                    while (nextTile.GetSide(Tile.SideName.Left) != lastRight &&
                           nextTile.GetSide(Tile.SideName.LeftReverse) != lastRight)
                    {
                        nextTile.Rotate();
                    }

                    if (nextTile.GetSide(Tile.SideName.Left) != lastRight)
                    {
                        nextTile.FlipVertically();
                    }
                }
                else
                {
                    var lastBottom = assembled[r - 1, c].GetSide(Tile.SideName.Bottom);
                    nextTile = sides[lastBottom].Single(e => !e.IsPlaced);
                    while (nextTile.GetSide(Tile.SideName.Top) != lastBottom &&
                           nextTile.GetSide(Tile.SideName.TopReverse) != lastBottom)
                    {
                        nextTile.Rotate();
                    }

                    if (nextTile.GetSide(Tile.SideName.Top) != lastBottom)
                    {
                        nextTile.FlipHorizontally();
                    }
                }
                assembled[r, c] = nextTile;
                nextTile.IsPlaced = true;
            }
        }

        const int size = 96;
        var grid = Enumerable.Range(0, size)
            .Select(_ => new char[size])
            .ToArray();

        for (var c = 0; c < gridSize; c++)
        {
            for (var i = 1; i < 9; i++)
            {
                for (var r = 0; r < gridSize; r++)
                {
                    for (var j = 1; j < 9; j++)
                    {
                        grid[c * 8 + i - 1][r * 8 + j - 1] = assembled[r, c].Data[j][i];
                    }
                }
            }
        }

        var waterCount = grid.Sum(x => x.Count(c => c == '#'));

        for (var i = 0; i < 8; i++)
        {
            var count = CountMonsters(grid);
            if (count > 0)
            {
                return waterCount - count * 15;
            }

            grid = i == 4
                ? grid.FlipRows()
                : grid.Rotate90DegreesRight();
        }

        return 0;
    }

    private int CountMonsters(char[][] input)
    {
        var cnt = 0;
        for (var r = 0; r < input.Length - monster.Length; r++)
        {
            for (var c = 0; c < input[r].Length - monster[0].Length; c++)
            {
                if (HasMonster(input, r, c))
                {
                    cnt++;
                }
            }
        }
        return cnt;
    }

    private bool HasMonster(char[][] grid, int r, int c)
    {
        for (var r2 = 0; r2 < monster.Length; r2++)
        {
            for (var c2 = 0; c2 < monster[0].Length; c2++)
            {
                if (monster[r2][c2] == '#' && grid[r + r2][c + c2] != '#')
                {
                    return false;
                }
            }
        }
        return true;
    }

    private class Tile(string[] lines)
    {
        public enum SideName { Top, TopReverse, Left, LeftReverse, Bottom, BottomReverse, Right, RightReverse }
        public int ID { get; } = int.Parse(lines[0][5..].TrimEnd(':'));
        public char[][] Data { get; private set; } = lines[1..].ToCharArray();
        public bool IsPlaced { get; set; }
        public IEnumerable<int> Sides => Enum.GetValues(typeof(SideName)).Cast<SideName>().Select(GetSide);

        public int GetSide(SideName name)
        {
            return name switch
            {
                SideName.Top => ParseNumber(new string(Enumerable.Range(0, 10).Select(r => Data[0][r]).ToArray())),
                SideName.TopReverse => ParseNumber(new string(Enumerable.Range(0, 10)
                    .Select(r => Data[0][r])
                    .Reverse()
                    .ToArray())),
                SideName.Left => ParseNumber(new string(Enumerable.Range(0, 10).Select(r => Data[r][0]).ToArray())),
                SideName.LeftReverse => ParseNumber(new string(Enumerable.Range(0, 10)
                    .Select(r => Data[r][0])
                    .Reverse()
                    .ToArray())),
                SideName.Bottom => ParseNumber(new string(Enumerable.Range(0, 10).Select(r => Data[9][r]).ToArray())),
                SideName.BottomReverse => ParseNumber(new string(Enumerable.Range(0, 10)
                    .Select(r => Data[9][r])
                    .Reverse()
                    .ToArray())),
                SideName.Right => ParseNumber(new string(Enumerable.Range(0, 10).Select(r => Data[r][9]).ToArray())),
                SideName.RightReverse => ParseNumber(new string(Enumerable.Range(0, 10)
                    .Select(r => Data[r][9])
                    .Reverse()
                    .ToArray())),
                _ => -1
            };
        }

        public void Rotate()
        {
            Data = Data.Rotate90DegreesRight();
        }

        public void FlipHorizontally()
        {
            Data = Data.FlipRows();
        }

        public void FlipVertically()
        {
            Data = Data.FlipCols();
        }

        public int Matches(IEnumerable<Tile> tiles)
        {
            return tiles.Count(t => t.ID != ID && t.Sides.Any(s => Sides.Contains(s)));
        }

        private static int ParseNumber(string text)
        {
            return Convert.ToInt32(text.Replace('#', '1').Replace('.', '0'), 2);
        }
    }
}