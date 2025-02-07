using AdventOfCode.Interfaces;
using AdventOfCode.Library.Array;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2019.Days;

public class Day07 : IRiddle
{
    private static int[]? inputTape;
    private static readonly int[] inputs = new int[5];

    public string SolveFirst()
    {
        var data = this.InputToText()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        var phases = Enumerable.Range(0, 5)
            .GetPermutations(5)
            .Select(x => x.ToArray())
            .ToArray();

        inputTape = data;

        var max = 0;
        foreach (var phase in phases)
        {
            var systemId = 0;

            foreach (var phaseSetting in phase)
            {
                var inputsTaken = 0;
                var systemIds = new[] { phaseSetting, systemId };

                var input = data.ToArray();

                for (var i = 0; i < input.Length;)
                {
                    var n = input[i];
                    var c = n / 100 % 10;
                    var b = n / 1000 % 10;
                    var a = n / 10000 % 10;
                    switch (n % 100)
                    {
                        case 1:
                            input[a == 0 ? input[i + 3] : i + 3] =
                                input[c == 0 ? input[i + 1] : i + 1] +
                                input[b == 0 ? input[i + 2] : i + 2];
                            i += 4;
                            break;
                        case 2:
                            input[a == 0 ? input[i + 3] : i + 3] =
                                input[c == 0 ? input[i + 1] : i + 1] *
                                input[b == 0 ? input[i + 2] : i + 2];
                            i += 4;
                            break;
                        case 3:
                            input[c == 0 ? input[i + 1] : i + 1] = systemIds[inputsTaken++];
                            i += 2;
                            break;
                        case 4:
                            systemId = input[c == 0 ? input[i + 1] : i + 1];
                            i = input.Length;
                            break;
                        case 5:
                            if (input[c == 0 ? input[i + 1] : i + 1] != 0)
                            {
                                i = input[b == 0 ? input[i + 2] : i + 2];
                            }
                            else
                            {
                                i += 3;
                            }

                            break;
                        case 6:
                            if (input[c == 0 ? input[i + 1] : i + 1] == 0)
                            {
                                i = input[b == 0 ? input[i + 2] : i + 2];
                            }
                            else
                            {
                                i += 3;
                            }

                            break;
                        case 7:
                            input[a == 0 ? input[i + 3] : i + 3] =
                                input[c == 0 ? input[i + 1] : i + 1] < input[b == 0 ? input[i + 2] : i + 2]
                                    ? 1
                                    : 0;
                            i += 4;
                            break;
                        case 8:
                            input[a == 0 ? input[i + 3] : i + 3] =
                                input[c == 0 ? input[i + 1] : i + 1] == input[b == 0 ? input[i + 2] : i + 2]
                                    ? 1
                                    : 0;
                            i += 4;
                            break;
                        case 99:
                            i = input.Length;
                            break;
                    }
                }
            }

            max = Math.Max(max, systemId);
        }

        return max.ToString();
    }

    public string SolveSecond()
    {
        inputTape = this.InputToText()
             .Split(',')
             .Select(int.Parse)
             .ToArray();

        return Enumerable.Range(5, 5).ToArray().GetPermutations(5).Select(x => x.ToArray()).Max(EmulateSequence).ToString();
    }

    private static int EmulateSequence(int[] phases)
    {
        Array.Fill(inputs, 0);
        var amps = phases
            .Select((p, i) =>
                Emulate(p, i).GetEnumerator())
            .ToArray();

        int lastOutput = 0, onAmp = 0;

        while (true)
        {
            if (!amps[onAmp].MoveNext())
            {
                break;
            }

            var input = amps[onAmp].Current;
            lastOutput = input;
            onAmp = (onAmp + 1) % 5;
            inputs[onAmp] = input;
        }

        return lastOutput;
    }

    private static IEnumerable<int> Emulate(int phase, int ampNum, int pc = 0)
    {
        var tape = inputTape!.ToArray();
        var readPhase = false;

        while (pc < tape.Length)
        {
            var n = tape[pc];
            var c = n / 100 % 10;
            var b = n / 1000 % 10;
            var a = n / 10000 % 10;
            switch (n % 100)
            {
                case 1:
                    tape[a == 0 ? tape[pc + 3] : pc + 3] =
                        tape[c == 0 ? tape[pc + 1] : pc + 1] +
                        tape[b == 0 ? tape[pc + 2] : pc + 2];
                    pc += 4;
                    break;
                case 2:
                    tape[a == 0 ? tape[pc + 3] : pc + 3] =
                        tape[c == 0 ? tape[pc + 1] : pc + 1] *
                        tape[b == 0 ? tape[pc + 2] : pc + 2];
                    pc += 4;
                    break;
                case 3:
                    tape[c == 0 ? tape[pc + 1] : pc + 1] = readPhase ? inputs[ampNum] : phase;
                    readPhase = true;
                    pc += 2;
                    break;
                case 4:
                    yield return tape[c == 0 ? tape[pc + 1] : pc + 1];
                    pc += 2;
                    break;
                case 5:
                    if (tape[c == 0 ? tape[pc + 1] : pc + 1] != 0)
                    {
                        pc = tape[b == 0 ? tape[pc + 2] : pc + 2];
                    }
                    else
                    {
                        pc += 3;
                    }

                    break;
                case 6:
                    if (tape[c == 0 ? tape[pc + 1] : pc + 1] == 0)
                    {
                        pc = tape[b == 0 ? tape[pc + 2] : pc + 2];
                    }
                    else
                    {
                        pc += 3;
                    }

                    break;
                case 7:
                    tape[a == 0 ? tape[pc + 3] : pc + 3] =
                        tape[c == 0 ? tape[pc + 1] : pc + 1] < tape[b == 0 ? tape[pc + 2] : pc + 2]
                            ? 1
                            : 0;
                    pc += 4;
                    break;
                case 8:
                    tape[a == 0 ? tape[pc + 3] : pc + 3] =
                        tape[c == 0 ? tape[pc + 1] : pc + 1] == tape[b == 0 ? tape[pc + 2] : pc + 2]
                            ? 1
                            : 0;
                    pc += 4;
                    break;
                case 99:
                    pc = tape.Length;
                    yield break;
            }
        }
    }

    private static (int op, int modeA, int modeB, int modeC) ParseMode(int modeOp)
    {
        var op = modeOp % 100;
        var modeC = modeOp / 100 % 10;
        var modeB = modeOp / 1000 % 10;
        var modeA = modeOp / 10000 % 10;
        return (op, modeA, modeB, modeC);
    }
}