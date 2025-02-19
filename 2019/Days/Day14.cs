using AdventOfCode.Interfaces;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day14 : IRiddle
{
    public string SolveFirst()
    {
        var reactions = this.InputToLines()
            .Select(l => l.Split([" => "], 0))
            .Select(a => new { Inputs = a[0].Split([", "], 0), Output = a[1].Split(' ') })
            .Select(t => new
            {
                Inputs = t.Inputs.Select(i => i.Split(' ')).ToDictionary(a => a[1], a => int.Parse(a[0])),
                Output = new KeyValuePair<string, int>(t.Output[1], int.Parse(t.Output[0]))
            })
            .ToDictionary(r => r.Output.Key, r => r);

        var deficits = new Dictionary<string, int> { { "FUEL", 1 } };
        while (deficits.Any(kvp => kvp.Key != "ORE" && kvp.Value > 0))
        {
            var fill = deficits.First(kvp => kvp.Key != "ORE" && kvp.Value > 0);
            var reaction = reactions[fill.Key];
            deficits[fill.Key] -= reaction.Output.Value;
            foreach (var input in reaction.Inputs.Where(input => !deficits.TryAdd(input.Key, input.Value)))
            {
                deficits[input.Key] += input.Value;
            }
        }

        return deficits["ORE"].ToString();
    }

    public string SolveSecond()
    {
        var reactions = this.InputToLines()
            .Select(l => l.Split([" => "], 0))
            .Select(a => new { Inputs = a[0].Split([", "], 0), Output = a[1].Split(' ') })
            .Select(t => new
            {
                Inputs = t.Inputs.Select(i => i.Split(' ')).ToDictionary(a => a[1], a => int.Parse(a[0])),
                Output = new KeyValuePair<string, int>(t.Output[1], int.Parse(t.Output[0]))
            })
            .ToDictionary(r => r.Output.Key, r => r);

        var stocks = new Dictionary<string, long> { { "ORE", 1000000000000 } };

        var mf = 1000000;
        while (mf > 0)
        {
            while (TryMake("FUEL", mf)) { }

            mf /= 10;
        }

        return stocks["FUEL"].ToString();

        bool TryMake(string target, long amount)
        {
            var reaction = reactions[target];
            var runs = (long)Math.Ceiling(amount / (double)reaction.Output.Value);
            if (reaction.Inputs.Any(input => stocks.GetValueOrDefault(input.Key, 0) < runs * input.Value && input.Key == "ORE"))
            {
                return false;
            }

            var stockBackup = stocks.ToDictionary(a => a.Key, a => a.Value);
            while (reaction.Inputs.Any(input => stocks.GetValueOrDefault(input.Key, 0) < runs * input.Value))
            {
                var mustMake = reaction.Inputs.First(input => stocks.GetValueOrDefault(input.Key, 0) < runs * input.Value);
                var need = runs * mustMake.Value - stocks.GetValueOrDefault(mustMake.Key, 0);
                if (TryMake(mustMake.Key, need)) continue;
                stocks = stockBackup;
                return false;
            }

            foreach (var input in reaction.Inputs.Where(input => !stocks.TryAdd(input.Key, -runs * input.Value)))
            {
                stocks[input.Key] += -runs * input.Value;
            }

            if (!stocks.TryAdd(target, runs * reaction.Output.Value))
            {
                stocks[target] += runs * reaction.Output.Value;
            }

            return true;
        }
    }
}