using AdventOfCode.Interfaces;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using AdventOfCode.Library.Input;

namespace AdventOfCode._2021.Days;

public class Day23 : IRiddle
{
    public string SolveFirst()
    {
        return SolveMinimumEnergyForFile(this.InputToLines()).ToString();

    }

    public string SolveSecond()
    {
        var input = this.InputToLines();
        return SolveMinimumEnergyForFile(
            input.Take(3)
            .Concat(["  #D#C#B#A#", "  #D#B#A#C#"])
            .Concat(input.Skip(3))
            .ToList())
            .ToString();
    }

    private int SolveMinimumEnergyForFile(IReadOnlyList<string> lines)
    {
        var input = Parse(lines);
        var goal = input with { Rooms = new string(input.Rooms.OrderBy(x => x).ToArray()) };

        return SolveMinimumEnergy(input, goal);
    }

    private static int SolveMinimumEnergy(State start, State goal)
    {
        var dist = new Dictionary<State, int> { [start] = 0 };
        var visited = new HashSet<State>();
        var queue = new PriorityQueue<State, int>();

        queue.EnqueueRange(dist.Select(x => (x.Key, x.Value)));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current)) continue;
            if (current == goal) break;

            var currentCost = dist[current];

            foreach (var (next, nextCost) in current.EnumMoves())
            {
                var alt = currentCost + nextCost;
                if (alt >= dist.GetValueOrDefault(next, int.MaxValue)) continue;
                dist[next] = alt;
                queue.Enqueue(next, alt);
            }
        }

        return dist[goal];
    }

    private static State Parse(IReadOnlyList<string> input)
    {
        var hall = new string('.', State.HallwayLength);
        var rooms = new[] { string.Empty, string.Empty, string.Empty, string.Empty };

        foreach (var line in input)
        {
            if (!line.Any(State.AmphipodTypes.Contains)) continue;
            var values = line.Where(char.IsLetter)
                .Select(x => x.ToString())
                .ToArray();

            for (var i = 0; i < rooms.Length; ++i)
            {
                rooms[i] += values[i];
            }
        }

        var roomsValue = string.Concat(rooms);
        return new State(hall, roomsValue, rooms[0].Length);
    }

    private record State(string Hall, string Rooms, int RoomSize)
    {
        private static readonly ImmutableArray<int> RoomPositions = [2, 4, 6, 8];

        public static readonly char[] AmphipodTypes = ['A', 'B', 'C', 'D'];

        public const int HallwayLength = 11;

        private string GetRoom(int index) => Rooms.Substring(index * RoomSize, RoomSize);

        public IEnumerable<(State state, int cost)> EnumMoves()
        {
            for (var i = 0; i < 4; ++i)
            {
                foreach (var pos in EnumOpenSpaces(i))
                {
                    if (!TryMoveOut(i, pos, out var updated, out var amphipod, out var steps)) continue;
                    var cost = steps * GetEnergyFactor(amphipod);
                    yield return (updated, cost);
                }
            }

            for (var i = 0; i < Hall.Length; ++i)
            {
                if (!TryMoveIn(i, out var updated, out var amphipod, out var steps)) continue;
                var cost = steps * GetEnergyFactor(amphipod);
                yield return (updated, cost);
            }
        }

        private bool TryMoveOut(int roomIndex,
            int targetPosition,
            [NotNullWhen(true)] out State? updated,
            out char amphipod,
            out int steps)
        {
            var room = GetRoom(roomIndex);
            var depth = room.IndexOfAny(AmphipodTypes);
            if (depth < 0)
            {
                updated = null;
                amphipod = '\0';
                steps = 0;
                return false;
            }

            steps = Math.Abs(targetPosition - RoomPositions[roomIndex]) + depth + 1;
            amphipod = room[depth];

            var updatedHall = Hall.Remove(targetPosition, 1).Insert(targetPosition, amphipod.ToString());
            var updatedRoom = room.Remove(depth, 1).Insert(depth, ".");

            updated = Update(updatedHall, roomIndex, updatedRoom);
            return true;
        }

        private bool TryMoveIn(int hallwayPosition,
            [NotNullWhen(true)] out State? updated,
            out char amphipod,
            out int steps)
        {
            updated = null;
            steps = 0;

            amphipod = Hall[hallwayPosition];
            var goalRoomIndex = Array.IndexOf(AmphipodTypes, amphipod);
            if (goalRoomIndex < 0)
            {
                return false;
            }

            var goalPosition = RoomPositions[goalRoomIndex];
            var start = goalPosition > hallwayPosition ? hallwayPosition + 1 : hallwayPosition - 1;
            var min = Math.Min(goalPosition, start);
            var max = Math.Max(goalPosition, start);
            if (Hall.Skip(min).Take(max - min + 1).Any(ch => ch != '.'))
            {
                return false;
            }

            var room = GetRoom(goalRoomIndex);
            var type = amphipod;
            if (room.Any(ch => ch != '.' && ch != type))
            {
                return false;
            }

            var depth = room.LastIndexOf('.');
            steps = max - min + 1 + depth + 1;

            var updatedHall = Hall.Remove(hallwayPosition, 1).Insert(hallwayPosition, ".");
            var updatedRoom = room.Remove(depth, 1).Insert(depth, amphipod.ToString());

            updated = Update(updatedHall, goalRoomIndex, updatedRoom);
            return true;
        }

        private State Update(string updatedHall, int roomIndex, string updatedRoom)
        {
            var updatedRooms = Rooms.Remove(roomIndex * RoomSize, RoomSize).Insert(roomIndex * RoomSize, updatedRoom);
            return this with { Hall = updatedHall, Rooms = updatedRooms };
        }

        private IEnumerable<int> EnumOpenSpaces(int roomIndex)
        {
            var position = RoomPositions[roomIndex];

            for (var i = position - 1; i >= 0 && Hall[i] == '.'; --i)
            {
                if (!RoomPositions.Contains(i))
                {
                    yield return i;
                }
            }

            for (var i = position + 1; i < Hall.Length && Hall[i] == '.'; ++i)
            {
                if (!RoomPositions.Contains(i))
                {
                    yield return i;
                }
            }
        }

        private static int GetEnergyFactor(char amphipod) =>
            amphipod switch
            {
                'A' => 1,
                'B' => 10,
                'C' => 100,
                'D' => 1000,
                _ => throw new ArgumentOutOfRangeException(nameof(amphipod))
            };
    }
}