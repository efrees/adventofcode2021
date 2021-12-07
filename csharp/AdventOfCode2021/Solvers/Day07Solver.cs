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
            var lines = Input.GetInputFromFile(InputFile)
                .Split(',')
                .Select(int.Parse)
                .OrderBy(x => x)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<int> input)
        {
            var min = int.MaxValue;
            var low = input.First();
            var high = input.Last();
            foreach (var target in Enumerable.Range(low, high-low))
            {
                var newSum = input.Sum(i => Math.Abs(i - target));
                if (newSum > min)
                {
                    break;
                }

                min = newSum;
            }

            return min;
        }

        private static int GetPart2Answer(List<int> input)
        {
            var min = int.MaxValue;
            var low = input.First();
            var high = input.Last();
            foreach (var target in Enumerable.Range(low, high-low))
            {
                var newSum = input.Sum(i => Cost(i, target));

                if (newSum > min)
                {
                    break;
                }
             
                min = newSum;
            }

            return min;
        }

        private static int Cost(int from, int target)
        {
            var diff = Math.Abs(from - target);
            return diff * (diff + 1) / 2;
        }
    }
}