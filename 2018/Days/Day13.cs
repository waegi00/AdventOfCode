using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;
using AdventOfCode.Library.String;

namespace AdventOfCode._2018.Days;

public class Day13 : IRiddle
{
    public string SolveFirst()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var carts = grid.FindAll(['^', '>', 'v', '<'])
            .Select(x => new Cart((x.i, x.j), Direction(grid[x.i][x.j]), 0))
            .ToList();

        while (true)
        {
            var newCarts = new List<Cart>();

            foreach (var newCart in carts.Select(cart => cart.Move(grid)))
            {
                if (newCarts.Any(x => x.Position == newCart.Position))
                {
                    return $"{newCart.Position.y},{newCart.Position.x}";
                }

                newCarts.Add(newCart);
            }

            carts = newCarts.OrderBy(x => x.Position.x).ThenBy(x => x.Position.y).ToList();
        }
    }

    public string SolveSecond()
    {
        var grid = this.InputToLines()
            .ToCharArray();

        var carts = grid.FindAll(['^', '>', 'v', '<'])
            .Select(x => new Cart((x.i, x.j), Direction(grid[x.i][x.j]), 0))
            .ToList();

        while (carts.Count > 1)
        {
            var newCarts = new List<Cart>();

            while (carts.Count > 0)
            {
                var newCart = carts[0].Move(grid);
                carts.RemoveAt(0);
                if (carts.Any(c => c.Position == newCart.Position))
                {
                    carts.Remove(carts.First(c => c.Position == newCart.Position));
                }
                else if (newCarts.All(c => c.Position != newCart.Position))
                {
                    newCarts.Add(newCart);
                }
                else
                {
                    newCarts.Remove(newCarts.First(c => c.Position == newCart.Position));
                }
            }

            carts = newCarts.OrderBy(x => x.Position.x).ThenBy(x => x.Position.y).ToList();
        }

        var cart = carts.First();
        return $"{cart.Position.y},{cart.Position.x}";
    }

    private record Cart((int x, int y) Position, (int vx, int vy) Direction, int IntersectionsHit)
    {
        public Cart Move(char[][] grid)
        {
            var position = (x: Position.x + Direction.vx, y: Position.y + Direction.vy);
            var direction = Direction;
            var hits = IntersectionsHit;

            switch (grid[position.x][position.y])
            {
                case '+':
                    direction = (hits % 3) switch
                    {
                        0 => TurnLeft(direction),
                        2 => TurnRight(direction),
                        _ => direction
                    };
                    hits++;
                    break;
                case '/':
                    direction = direction.vx == 0 ? TurnLeft(direction) : TurnRight(direction);
                    break;
                case '\\':
                    direction = direction.vy == 0 ? TurnLeft(direction) : TurnRight(direction);
                    break;
            }

            return new Cart(position, direction, hits);
        }
    }

    private static (int, int) Direction(char c)
    {
        return c switch
        {
            '^' => (-1, 0),
            '>' => (0, 1),
            'v' => (1, 0),
            '<' => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }

    private static char Direction((int, int) dir)
    {
        return dir switch
        {
            (-1, 0) => '^',
            (0, 1) => '>',
            (1, 0) => 'v',
            (0, -1) => '<',
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    private static (int, int) TurnLeft((int, int) val)
    {
        return (-val.Item2, val.Item1);
    }
    private static (int, int) TurnRight((int, int) val)
    {
        return (val.Item2, -val.Item1);
    }
}