using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day17Solver : ISolver
    {
        private const string Name = "Day 17";

        private const string InputFile = "day17input.txt";

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
            // x=85..145, y=-163..-108
            var xRange = (85, 145);
            var yRange = (-163, -108);

            var bestMaxY = 0;
            foreach (var x in Enumerable.Range(1, xRange.Item2))
            {
                foreach (var y in Enumerable.Range(yRange.Item1, 1000)) //arbitrary bound
                {
                    var maxY = TrackProbeMaxY((x, y), xRange, yRange);

                    if (maxY > bestMaxY)
                    {
                        bestMaxY = maxY;
                    }
                }
            }

            return bestMaxY;
        }

        private static int GetPart2Answer(List<string> input)
        {
            // x=85..145, y=-163..-108
            var xRange = (85, 145);
            var yRange = (-163, -108);

            var countSuccesses = 0;
            foreach (var x in Enumerable.Range(1, xRange.Item2))
            {
                foreach (var y in Enumerable.Range(yRange.Item1, 1000)) //arbitrary bound
                {
                    var maxY = TrackProbeMaxY((x, y), xRange, yRange);

                    if (maxY != -1)
                    {
                        countSuccesses++;
                    }
                }
            }

            return countSuccesses;
        }
        
        private static int TrackProbeMaxY((int x, int y) velocity, (int, int) xRange, (int, int) yRange)
        {
            var position = (x: 0, y: 0);
            var maxY = 0;
            while (true)
            {
                position = (position.x + velocity.x, position.y + velocity.y);
                var dx = velocity switch
                {
                    { x: > 0 } => -1,
                    { x: < 0 } => 1,
                    _ => 0
                };
                velocity = (velocity.x + dx, velocity.y - 1);

                maxY = Math.Max(maxY, position.y);
                if (InRange(position.x, xRange) && InRange(position.y, yRange))
                {
                    return maxY; // assuming we don't reach the highest point after the target.
                }

                if (velocity.x == 0 && !InRange(position.x, xRange)
                    || velocity.x < 0 && position.x < xRange.Item1
                    || velocity.x > 0 && position.x > xRange.Item2
                    || velocity.y < 0 && position.y < yRange.Item1)
                {
                    return -1;
                }
            }

            return -1;
        }

        private static bool InRange(int position, (int, int) range)
        {
            return position >= range.Item1 && position <= range.Item2;
        }
    }
}