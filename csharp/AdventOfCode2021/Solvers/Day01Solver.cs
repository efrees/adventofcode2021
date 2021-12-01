using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day01Solver : ISolver
    {
        private const string Name = "Day 1";
        private const string InputFile = "day01input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile).Select(int.Parse).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<int> depths)
        {
            var previous = depths.First();
            var increaseCount = 0;
            foreach (var next in depths)
            {
                if (previous < next)
                {
                    increaseCount++;
                }

                previous = next;
            }

            return increaseCount;
        }
        
        private static long GetPart2Answer(List<int> depths)
        {
            var previousSum = depths.Take(3).Sum();
            var increaseCount = 0;
            for (var i = 1; i < depths.Count - 2; i++)
            {
                var nextSum = depths[i] + depths[i + 1] + depths[i + 2];

                if (previousSum < nextSum)
                {
                    increaseCount++;
                }

                previousSum = nextSum;
            }

            return increaseCount;
        }
    }
}