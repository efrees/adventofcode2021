using Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day15Solver : ISolver
    {
        private const string Name = "Day 15";

        private const string InputFile = "day15input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            //Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<string> input)
        {
            var maxY = input.Count - 1;
            var maxX = input[0].Length - 1;
            var grid = ParseAsGrid(input);

            var start = (0, 0);
            var target = (maxX, maxY);

            var orderedFrontier = new BinaryHeap<int, SearchNode>();
            var visitedMins = new Dictionary<(int, int), int>();
            var startNode = new SearchNode
            {
                Position = start,
                Cost = 0
            };
            orderedFrontier.Add(GetHeuristicCost(start, target), startNode);
            while (orderedFrontier.Count > 0)
            {
                var next = orderedFrontier.Dequeue().Value;

                if (next.Position == target)
                {
                    return next.Cost;
                }

                if (visitedMins.ContainsKey(next.Position) && visitedMins[next.Position] < next.Cost)
                {
                    continue;
                }

                visitedMins[next.Position] = next.Cost;

                foreach (var adjacent in GetAdjacentPoints(next.Position))
                {
                    if (grid.ContainsKey(adjacent))
                    {
                        var nextCost = next.Cost + grid[adjacent];
                        orderedFrontier.Add(nextCost + GetHeuristicCost(adjacent, target), new SearchNode
                        {
                            Position = adjacent,
                            Cost = nextCost
                        });
                    }
                }
            }

            return -1;
        }

        public static int GetHeuristicCost((int, int) start, (int, int) target)
        {
            return target.Item1 - start.Item1 + target.Item2 - start.Item2;
        }

        private static Dictionary<(int x, int y), int> ParseAsGrid(List<string> input)
        {
            return input
                .SelectMany((line, y) => line.Select((ch, x) => (coords: (x, y), val: ch - '0')))
                .ToDictionary(pair => pair.coords, pair => pair.val);
        }

        private static IEnumerable<(int x, int y)> GetAdjacentPoints((int, int) point)
        {
            var adjacentOffsets = new[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };

            foreach (var offset in adjacentOffsets)
            {
                yield return (point.Item1 + offset.Item1, point.Item2 + offset.Item2);
            }
        }

        private class SearchNode
        {
            public (int, int) Position { get; set; }
            public int Cost { get; set; }
        }
    }
}