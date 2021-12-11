using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day11Solver : ISolver
    {
        private const string Name = "Day 11";

        private const string InputFile = "day11input.txt";

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
            var grid = input.SelectMany((line, y) => line.Select((ch, x) => (coords: (x, y), val: ch - '0')))
                .ToDictionary(pair => pair.coords, pair => pair.val);

            var flashCount = 0;
            for (var i = 0; i < 100; i++)
            {
                foreach (var (coords, value) in grid)
                {
                    grid[coords] = value + 1;
                }

                var flashedCoords = new HashSet<(int x, int y)>();
                foreach (var (coords, value) in grid)
                {
                    flashCount += CheckFlashCoords(grid, coords, flashedCoords);
                }

                foreach (var coords in flashedCoords)
                {
                    grid[coords] = 0;
                }
            }

            return flashCount;
        }

        private static int GetPart2Answer(List<string> input)
        {
            var grid = input.SelectMany((line, y) => line.Select((ch, x) => (coords: (x, y), val: ch - '0')))
                .ToDictionary(pair => pair.coords, pair => pair.val);

            var syncStep = -1;
            var currentStep = 1;
            while (syncStep == -1)
            {
                var flashCount = 0;
                foreach (var (coords, value) in grid)
                {
                    grid[coords] = value + 1;
                }

                var flashedCoords = new HashSet<(int x, int y)>();
                foreach (var (coords, value) in grid)
                {
                    flashCount += CheckFlashCoords(grid, coords, flashedCoords);
                }

                foreach (var coords in flashedCoords)
                {
                    grid[coords] = 0;
                }

                if (flashCount == grid.Keys.Count)
                {
                    syncStep = currentStep;
                }

                currentStep++;
            }

            return syncStep;
        }

        private static int CheckFlashCoords(Dictionary<(int x, int y), int> grid,
            (int x, int y) coords,
            HashSet<(int x, int y)> flashedCoords)
        {
            if (grid[coords] <= 9 || flashedCoords.Contains(coords))
            {
                return 0;
            }

            flashedCoords.Add(coords);
            var flashCount = 1;
            foreach (var adj in AdjacentCoords(coords, 9, 9))
            {
                grid[adj] += 1;
                flashCount += CheckFlashCoords(grid, adj, flashedCoords);
            }

            return flashCount;
        }

        private static IEnumerable<(int x, int y)> AdjacentCoords((int x, int y) coords, int maxX, int maxY)
        {
            foreach (var dx in Enumerable.Range(-1, 3))
            foreach (var dy in Enumerable.Range(-1, 3))
            {
                var newPoint = (x: coords.x + dx, y: coords.y + dy);
                if (dx == 0 && dy == 0)
                    continue;
                if (newPoint.x < 0 || newPoint.x > maxX
                                   || newPoint.y < 0 || newPoint.y > maxY)
                    continue;

                yield return newPoint;
            }
        }
    }
}