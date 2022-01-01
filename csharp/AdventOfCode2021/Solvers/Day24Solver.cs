using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day24Solver : ISolver
    {
        private const string Name = "Day 24";

        private const string InputFile = "day24input_analyzed.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private string GetPart1Answer(List<string> lines)
        {
            return lines[^2];
        }
        private string GetPart2Answer(List<string> lines)
        {
            return lines[^1];
        }
    }
}