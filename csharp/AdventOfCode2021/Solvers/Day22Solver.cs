using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Solvers
{
    internal class Day22Solver : ISolver
    {
        private const string Name = "Day 22";

        private const string InputFile = "day22input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .Select(Step.Parse)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            //Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<Step> input)
        {
            var onCount = 0;
            for (var i = -50; i <= 50; i++)
            {
                for (var j = -50; j <= 50; j++)
                {
                    for (var k = -50; k <= 50; k++)
                    {
                        if (input.LastOrDefault(s => s.IsInRange((i, j, k)))?.TurnOn == true)
                        {
                            onCount++;
                        }
                    }
                }
            }

            return onCount;
        }

        private class Step
        {
            public (int min, int max) XRange { get; set; }
            public (int min, int max) YRange { get; set; }
            public (int min, int max) ZRange { get; set; }
            public bool TurnOn { get; set; }

            public bool IsInRange((int x, int y, int z) point)
            {
                return XRange.min <= point.x && XRange.max >= point.x
                                             && YRange.min <= point.y && YRange.max >= point.y
                                             && ZRange.min <= point.z && ZRange.max >= point.z;
            }

            public static Step Parse(string line)
            {
                var split = line.Split(' ');
                var turnsOn = split[0] == "on";
                var rangeBoundaries = Regex.Match(split[1], @"x=(-?\d+)\.\.(-?\d+),y=(-?\d+)\.\.(-?\d+),z=(-?\d+)\.\.(-?\d+)").Groups;
                var xRange = (int.Parse(rangeBoundaries[1].Value), int.Parse(rangeBoundaries[2].Value));
                var yRange = (int.Parse(rangeBoundaries[3].Value), int.Parse(rangeBoundaries[4].Value));
                var zRange = (int.Parse(rangeBoundaries[5].Value), int.Parse(rangeBoundaries[6].Value));

                return new Step
                {
                    XRange = xRange,
                    YRange = yRange,
                    ZRange = zRange,
                    TurnOn = turnsOn
                };
            }

        }
    }
}