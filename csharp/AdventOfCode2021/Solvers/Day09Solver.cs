using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day09Solver : ISolver
    {
        private const string Name = "Day 9";

        private const string InputFile = "day09input.txt";

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
            var sum = 0;
            var adjacentOffsets = new[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };
            for(var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    var current = input[i][j];
                    var isLow = true;
                    foreach(var (k, m) in adjacentOffsets)
                    {
                        if (k + i < 0 || m + j< 0)
                        {
                            continue;
                        }

                        if (k + i >= input.Count || m + j >= input[i].Length)
                        {
                            continue;
                        }

                        if (input[i + k][j + m] <= current)
                        {
                            isLow = false;
                        }
                    }

                    if (isLow)
                    {
                        sum += current + 1 - '0';
                    }
                }
            }

            return sum;
        }

        private static int GetPart2Answer(List<string> input)
        {
            var sizes = GetBasinSizes(input).OrderByDescending(size => size);
            return sizes.Take(3).Aggregate((cur, next) => cur * next);
        }

        private static IEnumerable<int> GetBasinSizes(List<string> input)
        {
            var nextBasinId = -1;

            var intGrid = input.Select(line => line.Select(c => c - '0').ToArray()).ToArray();

            for (var i = 0; i < intGrid.Length; i++)
            {
                for (var j = 0; j < intGrid[i].Length; j++)
                {
                    if (intGrid[i][j] == 9 || intGrid[i][j] < 0)
                    {
                        continue;
                    }

                    yield return FloodFillCount(intGrid, nextBasinId, i, j);
                }
            }
        }

        private static int FloodFillCount(int[][] intGrid, int nextBasinId, int i, int j)
        {
            if (i < 0 || j < 0 || i >= intGrid.Length || j >= intGrid[0].Length)
            {
                return 0;
            }

            if (intGrid[i][j] == 9 || intGrid[i][j] < 0)
            {
                return 0;
            }

            var adjacentOffsets = new[]
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };
            intGrid[i][j] = nextBasinId;
            return 1 + adjacentOffsets.Sum(offset => FloodFillCount(intGrid, nextBasinId, i + offset.Item1, j + offset.Item2));
        }
    }
}