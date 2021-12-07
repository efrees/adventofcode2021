using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day07Solver : ISolver
    {
        private const string Name = "Day 7";

        private const string InputFile = "day07input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetInputFromFile(InputFile).Split(',').Select(long.Parse).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<long> input)
        {
            var min = input.Sum();
            foreach (var target in input) //lucky
            {
                var newSum = input.Sum(i => Math.Abs(i - target));
                if (newSum < min)
                {
                    min = newSum;
                }
            }

            return min;
        }

        private static long GetPart2Answer(List<long> input)
        {
            var min = long.MaxValue;
            foreach (var target in Enumerable.Range((int)input.Min(), (int)input.Max() - (int)input.Min()))
            {
                var newSum = input.Sum(i => Cost(i, target));
                if (newSum < min)
                {
                    min = newSum;
                }
            }

            return min;
        }

        private static long Cost(long from, long target)
        {
            var diff = Math.Abs(from - target);
            return diff * (diff + 1) / 2;
        }
    }
}