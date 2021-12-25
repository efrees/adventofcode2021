using AdventOfCode2021.Grid;
using Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day23Solver : ISolver
    {
        private const string Name = "Day 23";

        private const string InputFile = "day23input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<string> input)
        {
            var initialGrid = SparseGrid<char>.Parse(input, ch => ch);
            var initialAmphipods = initialGrid.GetAllCoordinates()
                .Where(coords => char.IsLetter(initialGrid.GetCell(coords)))
                .Select(coords => new Amphipod
                {
                    Type = initialGrid.GetCell(coords),
                    Position = coords
                })
                .ToList();

            // #############
            // #..X.X.X.X..#
            // ###.#.#.#.###
            //   #.#.#.#.#
            //   #########
            var forbiddenStoppingPoints = new HashSet<Point2D> { (3, 1), (5, 1), (7, 1), (9, 1) };

            var hallLocations = initialGrid.GetAllCoordinates()
                .Where(coords => initialGrid.GetCell(coords) == '.')
                .ToHashSet();
            var roomLocations = initialAmphipods.Select(a => a.Position).ToHashSet();

            var frontier = new BinaryHeap<int, SearchState>();
            frontier.Add(0, new SearchState
            {
                Amphipods = initialAmphipods
            });
            while (frontier.Count > 0)
            {
                var searchState = frontier.Dequeue().Value;

                if (IsFinalState(searchState.Amphipods))
                {
                    return searchState.Cost;
                }

                var indexedPositions = searchState.Amphipods
                    .ToDictionary(a => a.Position);

                foreach (var amphipod in searchState.Amphipods)
                {
                    if (IsInFinalPosition(amphipod) && !IsAboveDifferentType(indexedPositions, amphipod))
                    {
                        continue;
                    }

                    if (hallLocations.Contains(amphipod.Position))
                    {
                        var goalXPosition = GoalXPosition(amphipod.Type);
                        var goalYPosition = new[] { 3, 2 }.FirstOrDefault(y =>
                            !indexedPositions.TryGetValue((goalXPosition, y), out var other) || other.Type != amphipod.Type);

                        if (goalYPosition == 0)
                        {
                            continue;
                        }
                        var targetLocation = new Point2D(goalXPosition, goalYPosition);

                        var horizontalMove = GetHorizontalMoveRange(amphipod, targetLocation);
                        var canReachTarget = IsHallwayClearBetween(horizontalMove, indexedPositions)
                                             && CanReachHallwayFrom(targetLocation.Add((0, 1)), indexedPositions);

                        if (!canReachTarget)
                        {
                            continue;
                        }

                        var nextSearchState = CreateStateAfterMove(searchState, amphipod, targetLocation);
                        frontier.Enqueue(nextSearchState.Cost + HeuristicCostRemaining(nextSearchState.Amphipods), nextSearchState);
                    }
                    else
                    {
                        //currently in the wrong room or needs to move
                        if (!CanReachHallwayFrom(amphipod.Position, indexedPositions))
                        {
                            continue;
                        }

                        foreach (var targetLocation in hallLocations.Except(forbiddenStoppingPoints).Except(indexedPositions.Keys))
                        {
                            var canReachTarget = true;
                            var horizontalMove = GetHorizontalMoveRange(amphipod, targetLocation);
                            canReachTarget = IsHallwayClearBetween(horizontalMove, indexedPositions);
                            
                            if (!canReachTarget)
                            {
                                continue;
                            }

                            var nextSearchState = CreateStateAfterMove(searchState, amphipod, targetLocation);
                            frontier.Enqueue(nextSearchState.Cost + HeuristicCostRemaining(nextSearchState.Amphipods), nextSearchState);
                        }
                    }
                }
            }

            return -1;
        }

        private static int GetPart2Answer(List<string> input)
        {
            input.Insert(3, "  #D#C#B#A#");
            input.Insert(4, "  #D#B#A#C#");

            var initialGrid = SparseGrid<char>.Parse(input, ch => ch);
            var initialAmphipods = initialGrid.GetAllCoordinates()
                .Where(coords => char.IsLetter(initialGrid.GetCell(coords)))
                .Select(coords => new Amphipod
                {
                    Type = initialGrid.GetCell(coords),
                    Position = coords
                })
                .ToList();

            var forbiddenStoppingPoints = new HashSet<Point2D> { (3, 1), (5, 1), (7, 1), (9, 1) };

            var hallLocations = initialGrid.GetAllCoordinates()
                .Where(coords => initialGrid.GetCell(coords) == '.')
                .ToHashSet();
            var roomLocations = initialAmphipods.Select(a => a.Position).ToHashSet();

            var frontier = new BinaryHeap<int, SearchState>();
            frontier.Add(0, new SearchState
            {
                Amphipods = initialAmphipods
            });
            while (frontier.Count > 0)
            {
                var searchState = frontier.Dequeue().Value;

                if (IsFinalState(searchState.Amphipods))
                {
                    return searchState.Cost;
                }

                var indexedPositions = searchState.Amphipods
                    .ToDictionary(a => a.Position);

                foreach (var amphipod in searchState.Amphipods)
                {
                    if (IsInFinalPosition(amphipod) && !IsAboveDifferentType(indexedPositions, amphipod))
                    {
                        continue;
                    }

                    if (hallLocations.Contains(amphipod.Position))
                    {
                        var goalXPosition = GoalXPosition(amphipod.Type);
                        var goalYPosition = new[] { 5, 4, 3, 2 }.FirstOrDefault(y =>
                            !indexedPositions.TryGetValue((goalXPosition, y), out var other) || other.Type != amphipod.Type);
                        if (goalYPosition == 0)
                        {
                            continue;
                        }
                        var targetLocation = new Point2D(goalXPosition, goalYPosition);

                        var horizontalMove = GetHorizontalMoveRange(amphipod, targetLocation);
                        var canReachTarget = IsHallwayClearBetween(horizontalMove, indexedPositions)
                            && CanReachHallwayFrom(targetLocation.Add((0, 1)), indexedPositions);

                        if (!canReachTarget)
                        {
                            continue;
                        }

                        var nextSearchState = CreateStateAfterMove(searchState, amphipod, targetLocation);
                        frontier.Enqueue(nextSearchState.Cost + HeuristicCostRemaining(nextSearchState.Amphipods), nextSearchState);
                    }
                    else
                    {
                        //currently in the wrong room or needs to move
                        if (!CanReachHallwayFrom(amphipod.Position, indexedPositions))
                        {
                            continue;
                        }

                        foreach (var targetLocation in hallLocations.Except(forbiddenStoppingPoints).Except(indexedPositions.Keys))
                        {
                            var horizontalMove = GetHorizontalMoveRange(amphipod, targetLocation);

                            if (!IsHallwayClearBetween(horizontalMove, indexedPositions))
                            {
                                continue;
                            }

                            var nextSearchState = CreateStateAfterMove(searchState, amphipod, targetLocation);
                            frontier.Enqueue(nextSearchState.Cost + HeuristicCostRemaining(nextSearchState.Amphipods), nextSearchState);
                        }
                    }
                }
            }

            return -1;
        }

        private static SearchState CreateStateAfterMove(SearchState searchState, Amphipod amphipod, Point2D targetLocation)
        {
            var moveDistance = (int)targetLocation
                .Subtract(amphipod.Position)
                .ManhattanDistance(Point2D.Origin);
            var moveCost = moveDistance * MoveCost(amphipod.Type);
            var updatedAmphipods = searchState.Amphipods.Where(a => a != amphipod).ToList();
            updatedAmphipods.Add(new Amphipod
            {
                Position = targetLocation,
                Type = amphipod.Type
            });

            var actualCost = searchState.Cost + moveCost;
            var nextSearchState = new SearchState
            {
                Amphipods = updatedAmphipods,
                Cost = actualCost
            };
            return nextSearchState;
        }

        private static bool IsHallwayClearBetween((long min, long max) horizontalMove, Dictionary<Point2D, Amphipod> indexedPositions)
        {
            for (var i = horizontalMove.min; i <= horizontalMove.max; i++)
            {
                if (indexedPositions.ContainsKey((i, 1)))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CanReachHallwayFrom(Point2D startingPosition, Dictionary<Point2D, Amphipod> indexedPositions)
        {
            for (var j = startingPosition.Y - 1; j >= 1; j--)
            {
                if (indexedPositions.ContainsKey((startingPosition.X, j)))
                {
                    return false;
                }
            }

            return true;
        }

        private static int HeuristicCostRemaining(IList<Amphipod> amphipods)
        {
            return amphipods
                .Select(a => MoveCost(a.Type) * (int)Math.Abs(a.Position.X - GoalXPosition(a.Type)))
                .Sum();
        }

        private static (long min, long max) GetHorizontalMoveRange(Amphipod amphipod, (long X, long Y) targetRoomLocation)
        {
            return amphipod.Position.X < targetRoomLocation.X
                ? (amphipod.Position.X + 1, targetRoomLocation.X)
                : (targetRoomLocation.X, amphipod.Position.X - 1);
        }

        private static bool IsAboveDifferentType(Dictionary<Point2D, Amphipod> indexedPositions, Amphipod amphipod)
        {
            return Enumerable.Range(1, 3)
                .Any(offset => indexedPositions.TryGetValue((amphipod.Position.X, amphipod.Position.Y + offset), out var other)
                   && other.Type != amphipod.Type);
        }

        private static bool IsFinalState(IList<Amphipod> amphipods)
        {
            return amphipods.All(IsInFinalPosition);
        }

        private static bool IsInFinalPosition(Amphipod amphipod)
        {
            return amphipod.Position.Y is 2 or 3 or 4 or 5
                   && amphipod.Position.X == GoalXPosition(amphipod.Type);
        }

        private static long GoalXPosition(char type)
        {
            return type switch
            {
                'A' => 3,
                'B' => 5,
                'C' => 7,
                'D' => 9
            };
        }

        private static int MoveCost(char type)
        {
            return type switch
            {
                'A' => 1,
                'B' => 10,
                'C' => 100,
                'D' => 1000
            };
        }

        private class SearchState
        {
            public IList<Amphipod> Amphipods { get; set; }
            public int Cost { get; set; }
        }

        private class Amphipod
        {
            public char Type { get; init; }
            public Point2D Position { get; init; }
        }
    }
}