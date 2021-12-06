using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day06Solver : ISolver
    {
        private const string Name = "Day 6";

        private const string InputFile = "day06input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetInputFromFile(InputFile).Split(',').Select(long.Parse).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<long> timers)
        {
            var groups = Enumerable.Range(0, 9).Select(timer => new FishGroup { Timer = timer, Count = 0 }).ToList();

            timers.ForEach(t => groups.First(g => g.Timer == t).Count++);

            RunGenerations(groups, 80);

            return groups.Sum(g => g.Count);
        }

        private static long GetPart2Answer(List<long> timers)
        {
            var groups = Enumerable.Range(0, 9).Select(timer => new FishGroup { Timer = timer, Count = 0 }).ToList();

            timers.ForEach(t => groups.First(g => g.Timer == t).Count++);

            RunGenerations(groups, 256);

            return groups.Sum(g => g.Count);
        }

        private static void RunGenerations(List<FishGroup> groups, int generationCount)
        {
            for (var generation = 0; generation < generationCount; generation++)
            {
                var newCount = 0L;
                foreach (var group in groups)
                {
                    if (group.Timer == 0)
                    {
                        newCount += group.Count;
                        group.Timer = 6;
                    }
                    else
                    {
                        group.Timer--;
                    }
                }

                if (newCount > 0)
                {
                    groups.Add(new FishGroup { Timer = 8, Count = newCount });
                }

                //Currently, I'm leaving duplicate 6's
            }
        }

        private class FishGroup
        {
            public int Timer { get; set; }
            public long Count { get; set; }
        }
    }
}