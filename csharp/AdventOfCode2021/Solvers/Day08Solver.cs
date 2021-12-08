using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day08Solver : ISolver
    {
        private const string Name = "Day 8";

        private const string InputFile = "day08input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .Select(DisplayData.Parse)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<DisplayData> input)
        {
            return input.SelectMany(d => d.DisplayedSignals).Count(s =>
                s.Length == 7
                || s.Length == 4
                || s.Length == 3
                || s.Length == 2
            );
        }

        private static int GetPart2Answer(List<DisplayData> input)
        {
            var sum = 0;
            foreach (var data in input)
            {
                var mapping = DeduceMapping(data);

                var result = 0;
                foreach (var digit in data.DisplayedSignals)
                {
                    result *= 10;
                    result += mapping[NormalizedKey(digit)];
                }

                sum += result;
            }

            return sum;
        }

        private static Dictionary<string, int> DeduceMapping(DisplayData data)
        {
            // Ambiguous ones
            // 5s: 2, 3, 5
            // 6s: 0, 6, 9

            var allSignals = data.UniqueSignals;
            var one = allSignals.Single(s => s.Length == 2);
            var four = allSignals.Single(s => s.Length == 4);
            var seven = allSignals.Single(s => s.Length == 3);
            var eight = allSignals.Single(s => s.Length == 7);

            // 9 is a 4 overlaid with 7, plus one segment. The other 6-segment ones have two differences
            var nine = allSignals.Single(s => s.Length == 6 && s.Except(four).Except(seven).Count() == 1);
            // 6 is the 6 segment digit that doesn't contain a whole 7
            var six = allSignals.Single(s => s.Length == 6 && s.Except(seven).Count() == 4);
            var zero = allSignals.Except(new[] { six, nine }).Single(s => s.Length == 6);

            // 2 is the only 5-segment number with the bottom-left (the one segment missing from a 9)
            var two = allSignals.Single(s => s.Length == 5 && s.Except(nine).Count() == 1);
            // 3 has only one difference from 2
            var three = allSignals.Single(s => s.Length == 5 && s.Except(two).Count() == 1);
            // 5 has two differences from 2
            var five = allSignals.Single(s => s.Length == 5 && s.Except(two).Count() == 2);

            var mapping = new Dictionary<string, int>
            {
                { NormalizedKey(zero), 0 },
                { NormalizedKey(one), 1 },
                { NormalizedKey(two), 2 },
                { NormalizedKey(three), 3 },
                { NormalizedKey(four), 4 },
                { NormalizedKey(five), 5 },
                { NormalizedKey(six), 6 },
                { NormalizedKey(seven), 7 },
                { NormalizedKey(eight), 8 },
                { NormalizedKey(nine), 9 },
            };
            return mapping;
        }

        private static string NormalizedKey(string characters)
        {
            return new string(characters.OrderBy(c => c).ToArray());
        }

        private class DisplayData
        {
            public IList<string> UniqueSignals { get; set; }
            public IList<string> DisplayedSignals { get; set; }

            public static DisplayData Parse(string line)
            {
                var halves = line.Split(" | ");
                return new DisplayData
                {
                    UniqueSignals = halves[0].Split(' '),
                    DisplayedSignals = halves[1].Split(' '),
                };
            }
        }
    }
}