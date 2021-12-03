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
            var result1 = mostCommonBits
                .Select(on => on
                    ? '1'
                    : '0')
                .ToArray();
            var result2 = mostCommonBits
                .Select(on => on
                    ? '0'
                    : '1')
                .ToArray();

            var gamma = Convert.ToInt32(new string(result1), 2);
            var epsilon = Convert.ToInt32(new string(result2), 2);

            return gamma * epsilon;
        }

        private static long GetPart2Answer(List<string> lines)
        {
            var oxygenLines = lines.ToList();
            for (var i = 0; i < lines.First().Length; i++)
            {
                var oneCounts = GetOneBitCounts(oxygenLines);
                oxygenLines = oxygenLines.Where(line =>
                    oneCounts[i] * 2 >= oxygenLines.Count && line[i] == '1'
                    || oneCounts[i] * 2 < oxygenLines.Count && line[i] == '0'
                ).ToList();

                if (oxygenLines.Count == 1)
                {
                    break;
                }
            }

            var co2Lines = lines.ToList();
            for (var i = 0; i < lines.First().Length; i++)
            {
                var oneCounts = GetOneBitCounts(co2Lines);
                co2Lines = co2Lines.Where(line =>
                    oneCounts[i] * 2 >= co2Lines.Count && line[i] == '0'
                    || oneCounts[i] * 2 < co2Lines.Count && line[i] == '1'
                ).ToList();

                if (co2Lines.Count == 1)
                {
                    break;
                }
            }

            var oxygenRating = Convert.ToInt32(new string(oxygenLines.Single()), 2);
            var co2Rating = Convert.ToInt32(new string(co2Lines.Single()), 2);

            return oxygenRating * co2Rating;
        }

        private static IEnumerable<bool> GetMostCommonBits(List<string> lines)
        {
            var oneCounts = GetOneBitCounts(lines);

            var mostCommonBits = Enumerable.Range(0, lines.First().Length)
                .Select(position => oneCounts[position] >= lines.Count / 2);
            return mostCommonBits;
        }

        private static Dictionary<int, int> GetOneBitCounts(List<string> lines)
        {
            var oneCounts = new Dictionary<int, int>();

            foreach (var line in lines)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (!oneCounts.ContainsKey(i))
                    {
                        oneCounts[i] = 0;
                    }

                    var bit = line[i];

                    if (bit == '1')
                        oneCounts[i]++;
                }
            }

            return oneCounts;
        }
    }
}