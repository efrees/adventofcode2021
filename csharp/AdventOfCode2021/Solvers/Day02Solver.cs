using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day02Solver : ISolver
    {
        private const string Name = "Day 2";
        private const string InputFile = "day02input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<string> instructions)
        {
            var x = 0;
            var depth = 0;
            foreach (var next in instructions)
            {
                var (direction, distance) = ParseInstruction(next);

                switch (direction)
                {
                    case "forward":
                        x += distance;
                        break;
                    case "up":
                        depth -= distance;
                        break;
                    case "down":
                        depth += distance;
                        break;
                }
            }

            return x * depth;
        }

        private static long GetPart2Answer(List<string> instructions)
        {
            var x = 0;
            var depth = 0;
            var aim = 0;
            foreach (var next in instructions)
            {
                var (direction, distance) = ParseInstruction(next);

                switch (direction)
                {
                    case "forward":
                        x += distance;
                        depth += aim * distance;
                        break;
                    case "up":
                        aim -= distance;
                        break;
                    case "down":
                        aim += distance;
                        break;
                }
            }

            return x * depth;
        }

        private static (string direction, int distance) ParseInstruction(string next)
        {
            var parts = next.Split(' ');
            var direction = parts[0];
            var distance = int.Parse(parts[1]);
            return (direction, distance);
        }
    }
}