using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2024.Days;

public class Day12 : IRiddle
{
    public string SolveFirst()
    {
        var input = this.InputToLines().ToCharArray();
        var visited = input.Select(r => r.Select(_ => false).ToArray()).ToArray();

        var res = 0;

        while (visited.Any(x => x.Any(y => !y)))
        {
            var area = 0;
            var perimeter = 0;

            HashSet<(int i, int j)> neighbours = [visited.FindFirst(false)];
            while (neighbours.Count > 0)
            {
                var (i, j) = neighbours.First();
                neighbours.Remove((i, j));
                visited[i][j] = true;
                var newNeighbours = input.Neighbours(i, j, true, true, false).Where(x => x.Item1 == input[i][j]).ToList();
                area++;
                perimeter += 4 - newNeighbours.Count;

                foreach (var (_, (ni, nj)) in newNeighbours)
                {
                    if (!visited[ni][nj])
                    {
                        neighbours.Add((ni, nj));
                    }
                }
            }
            res += area * perimeter;
        }

        return res.ToString();
    }

    public string SolveSecond()
    {
        var input = this.InputToLines().ToCharArray();
        var visited = input.Select(r => r.Select(_ => false).ToArray()).ToArray();

        var res = 0;

        while (visited.Any(x => x.Any(y => !y)))
        {
            var area = 0;
            var sides = 0;

            var group = new HashSet<(int i, int j)>();
            HashSet<(int i, int j)> toVisit = [visited.FindFirst(false)];
            while (toVisit.Count > 0)
            {
                var (i, j) = toVisit.First();
                toVisit.Remove((i, j));
                group.Add((i, j));
                visited[i][j] = true;
                var newNeighbours = input.Neighbours(i, j, true, true, false).Where(x => x.Item1 == input[i][j]).ToList();
                area++;

                foreach (var (_, (ni, nj)) in newNeighbours)
                {
                    if (!visited[ni][nj])
                    {
                        toVisit.Add((ni, nj));
                    }
                }
            }

            foreach (var (i, j) in group)
            {
                var horizontal = input.Neighbours(i, j, true, false, false, true).ToList();
                var vertical = input.Neighbours(i, j, false, true, false, true).ToList();
                sides += horizontal.Count(x => !group.Contains(x.Item2)) * vertical.Count(x => !group.Contains(x.Item2));
                sides += vertical.Where(x => group.Contains(x.Item2)).Sum(x => horizontal.Count(y => group.Contains(y.Item2) && !group.Contains((x.Item2.Item1, y.Item2.Item2))));
            }


            res += area * sides;
        }

        return res.ToString();
    }
}