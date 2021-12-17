using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Solvers
{
    internal class Day17Solver : ISolver
    {
        private const string Name = "Day 17";

        private const string InputFile = "day17input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetInputFromFile(InputFile);

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(string input)
        {
            var targetRange = ParseTargetRange(input);

            var bestMaxY = 0;
            foreach (var velocity in GenerateCandidateVelocities(targetRange))
            {
                var maxY = TrackProbeMaxY(velocity, targetRange);

                if (maxY > bestMaxY)
                {
                    bestMaxY = maxY;
                }
            }

            return bestMaxY;
        }

        private static int GetPart2Answer(string input)
        {
            var targetRange = ParseTargetRange(input);

            var countSuccesses = 0;
            foreach (var (x, y) in GenerateCandidateVelocities(targetRange))
            {
                var maxY = TrackProbeMaxY((x, y), targetRange);

                if (maxY != -1)
                {
                    countSuccesses++;
                }
            }

            return countSuccesses;
        }

        private static (int minX, int maxX, int minY, int maxY) ParseTargetRange(string input)
        {
            var groups = Regex.Match(input, "target area: x=(-?\\d+)..(-?\\d+), y=(-?\\d+)..(-?\\d+)").Groups;
            return (
                int.Parse(groups[1].Value),
                int.Parse(groups[2].Value),
                int.Parse(groups[3].Value),
                int.Parse(groups[4].Value)
            );
        }

        private static IEnumerable<(int x, int y)> GenerateCandidateVelocities((int minX, int maxX, int minY, int maxY) targetRange)
        {
            foreach (var x in Enumerable.Range(1, targetRange.maxX))
            {
                foreach (var y in Enumerable.Range(targetRange.minY, 1000)) //arbitrary bound
                {
                    yield return (x, y);
                }
            }
        }

        private static int TrackProbeMaxY((int x, int y) velocity, (int minX, int maxX, int minY, int maxY) targetRange)
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
                if (InRange(position.x, (targetRange.minX, targetRange.maxX)) && InRange(position.y, (targetRange.minY, targetRange.maxY)))
                {
                    return maxY; // assuming we don't reach the highest point after the target.
                }

                if (velocity.x <= 0 && position.x < targetRange.minX
                    || velocity.x >= 0 && position.x > targetRange.maxX
                    || velocity.y < 0 && position.y < targetRange.minY)
                {
                    break;
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