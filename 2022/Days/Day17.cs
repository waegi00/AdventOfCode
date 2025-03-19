using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day17 : IRiddle
{
    public string SolveFirst()
    {
        return Solve(this.InputToText()).ToString();
    }

    public string SolveSecond()
    {
        return Solve(this.InputToText(), true).ToString();
    }


    private static long Solve(string tape, bool isPart2 = false)
    {
        var map = new Dictionary<(int X, int Y), int>();

        string[][] blocks = [["@@@@"], [" @ ", "@@@", " @ "], ["@@@", "  @", "  @"], ["@", "@", "@", "@"], ["@@", "@@"]];

        var index = 0;
        var maxY = 0;
        var inPlay = false;
        var (blockX, blockY) = (0, 0);
        var nth = 0L;
        var block = 0;
        var current = blocks[block];
        var revisit = new Dictionary<(int Tape, int Shape), (long Rocks, long Height)>();

        while (true)
        {
            if (!inPlay)
            {
                blockX = 2;
                blockY = (map.Count > 1 ? maxY : -1) + 4;
                current = blocks[block];
                inPlay = true;
                nth++;
                if (nth == 2023 && !isPart2)
                {
                    return maxY + 1;
                }
            }

            var next = blockX;
            if (tape[index] == '<')
            {
                next--;
            }
            else
            {
                next++;
            }

            if (0 <= next && next + current[0].Length <= 7 && CheckCollision(next, blockY))
            {
                blockX = next;
            }

            index = (index + 1) % tape.Length;

            next = blockY - 1;
            if (0 <= next && CheckCollision(blockX, next))
            {
                blockY = next;
            }
            else
            {
                for (var y = 0; y < blocks[block].Length; y++)
                {
                    for (var x = 0; x < blocks[block][y].Length; x++)
                    {
                        var place = blocks[block][y][x] == '@';
                        if (place) map[(blockX + x, blockY + y)] = block;
                        maxY = Math.Max(maxY, blockY + y);
                    }
                }

                block = (block + 1) % blocks.Length;
                inPlay = false;

                if (revisit.ContainsKey((index, block)))
                {
                    var last = revisit[(index, block)];
                    var cycle = nth - last.Rocks;
                    var adds = maxY + 1 - last.Height;
                    var remaining = 1000000000000 - nth - 1;
                    var combo = (remaining / (cycle) + 1);
                    if (nth + combo * cycle == 1000000000000)
                    {
                        return maxY + 1 + combo * adds;
                    }
                }
                else
                {
                    revisit[(index, block)] = (nth, maxY + 1);
                }
            }
        }

        bool CheckCollision(int bx, int by)
        {
            var ok = true;
            for (var y = by; y < by + current.Length; y++)
            {
                for (var x = bx; x < bx + current[y - by].Length; x++)
                {
                    var read = map.GetValueOrDefault((x, y), -1);
                    if (by > y || y > by + current.Length - 1 || bx > x || x > bx + current[y - by].Length - 1) continue;
                    if (current[y - by][x - bx] == '@' && read != -1)
                    {
                        ok = false;
                    }
                }
            }

            return ok;
        }
    }
}