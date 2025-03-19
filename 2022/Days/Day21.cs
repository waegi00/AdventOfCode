using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2022.Days;

public class Day21 : IRiddle
{
    public string SolveFirst()
    {
        return Parse(this.InputToLines(), "root")
            .Simplify()
            .ToString()!;
    }

    public string SolveSecond()
    {
        var expression = (Eq)Parse(this.InputToLines(), "root", true);

        while (expression.left is not Var)
        {
            expression = Solve(expression);
        }

        return expression.right.ToString()!;
    }

    private static Eq Solve(Eq eq) =>
        eq.left switch
        {
            Op(Const l, "+", { } r) => new Eq(r, new Op(eq.right, "-", l).Simplify()),
            Op(Const l, "*", { } r) => new Eq(r, new Op(eq.right, "/", l).Simplify()),
            Op({ } l, "+", { } r) => new Eq(l, new Op(eq.right, "-", r).Simplify()),
            Op({ } l, "-", { } r) => new Eq(l, new Op(eq.right, "+", r).Simplify()),
            Op({ } l, "*", { } r) => new Eq(l, new Op(eq.right, "/", r).Simplify()),
            Op({ } l, "/", { } r) => new Eq(l, new Op(eq.right, "*", r).Simplify()),
            Const => new Eq(eq.right, eq.left),
            _ => eq
        };

    private static Expression Parse(string[] lines, string root, bool isPart2 = false)
    {
        var context = new Dictionary<string, string[]>();
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            context[parts[0].TrimEnd(':')] = parts.Skip(1).ToArray();
        }

        return BuildExpression(root);

        Expression BuildExpression(string name)
        {
            var parts = context[name];
            if (isPart2)
            {
                switch (name)
                {
                    case "humn":
                        return new Var("humn");
                    case "root":
                        return new Eq(BuildExpression(parts[0]), BuildExpression(parts[2]));
                }
            }
            if (parts.Length == 1)
            {
                return new Const(long.Parse(parts[0]));
            }

            return new Op(BuildExpression(parts[0]), parts[1], BuildExpression(parts[2]));
        }
    }

    private interface Expression
    {
        Expression Simplify();
    }

    private record Const(long Value) : Expression
    {
        public override string ToString() => Value.ToString();
        public Expression Simplify() => this;
    }

    private record Var(string name) : Expression
    {
        public override string ToString() => name;
        public Expression Simplify() => this;
    }

    private record Eq(Expression left, Expression right) : Expression
    {
        public override string ToString() => $"{left} == {right}";
        public Expression Simplify() => new Eq(left.Simplify(), right.Simplify());
    }

    private record Op(Expression left, string op, Expression right) : Expression
    {
        public override string ToString() => $"({left}) {op} ({right})";
        public Expression Simplify()
        {
            return (left.Simplify(), op, right.Simplify()) switch
            {
                (Const l, "+", Const r) => new Const(l.Value + r.Value),
                (Const l, "-", Const r) => new Const(l.Value - r.Value),
                (Const l, "*", Const r) => new Const(l.Value * r.Value),
                (Const l, "/", Const r) => new Const(l.Value / r.Value),
                ({ } l, _, { } r) => new Op(l, op, r),
            };
        }
    }
}