using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day03Solver : ISolver
    {
        private const string Name = "Day 3";

        private const string InputFile = "day03input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<string> lines)
        {
            var mostCommonBits = GetMostCommonBits(lines);
            var result1 = mostCommonBits;
            var result2 = mostCommonBits
                .Select(bit => bit == '1'
                    ? '0'
                    : '1')
                .ToArray();

            var gamma = Convert.ToInt32(new string(result1), 2);
            var epsilon = Convert.ToInt32(new string(result2), 2);

            return gamma * epsilon;
        }

        private static long GetPart2Answer(List<string> lines)
        {
            var numberOfBits = lines.First().Length;
            var oxygenLines = lines.ToList();
            for (var i = 0; i < numberOfBits && oxygenLines.Count > 1; i++)
            {
                var mostCommonBits = GetMostCommonBits(oxygenLines);
                oxygenLines = oxygenLines.Where(line => mostCommonBits[i] == line[i]).ToList();
            }

            var co2Lines = lines.ToList();
            for (var i = 0; i < numberOfBits && co2Lines.Count > 1; i++)
            {
                var mostCommonBits = GetMostCommonBits(co2Lines);
                co2Lines = co2Lines.Where(line => mostCommonBits[i] != line[i]).ToList();
            }

            var oxygenRating = Convert.ToInt32(new string(oxygenLines.Single()), 2);
            var co2Rating = Convert.ToInt32(new string(co2Lines.Single()), 2);

            return oxygenRating * co2Rating;
        }

        private static char[] GetMostCommonBits(List<string> lines)
        {
            var oneCounts = GetOneBitCounts(lines);

            return Enumerable.Range(0, lines.First().Length)
                .Select(position => oneCounts[position] * 2 >= lines.Count)
                .Select(on => on ? '1' : '0')
                .ToArray();
        }

        private static Dictionary<int, int> GetOneBitCounts(List<string> lines)
        {
            var oneCounts = new Dictionary<int, int>();
            var numberOfBits = lines.First().Length;

            for (var i = 0; i < numberOfBits; i++)
            {
                oneCounts[i] = lines.Count(line => line[i] == '1');
            }

            return oneCounts;
        }
    }
}