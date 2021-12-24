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

            lines.Reverse();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<Step> reversedSteps)
        {
            var rangeOfInterest = new Step
            {
                XRange = (-50, 50),
                YRange = (-50, 50),
                ZRange = (-50, 50)
            };
            var filteredSteps = reversedSteps.Where(step => step.Overlaps(rangeOfInterest)).ToList();
            return SplitOverlappingSteps(filteredSteps)
                .Where(step => step.TurnOn)
                .Select(step => step.GetSize())
                .Sum();
        }

        private static long GetPart2Answer(List<Step> reversedSteps)
        {
            return SplitOverlappingSteps(reversedSteps)
                .Where(step => step.TurnOn)
                .Select(step => step.GetSize())
                .Sum();
        }

        private static IEnumerable<Step> SplitOverlappingSteps(List<Step> reversedSteps)
        {
            var processedSteps = new List<Step>();
            foreach (var step in reversedSteps)
            {
                processedSteps.AddRange(GetNonOverlappingRegions(step, processedSteps));
            }

            return processedSteps;
        }

        private static IEnumerable<Step> GetNonOverlappingRegions(Step stepToProcess, List<Step> processedSteps)
        {
            var stepsToProcess = new List<Step>(new[] { stepToProcess });
            foreach (var processed in processedSteps.TakeWhile(_ => stepsToProcess.Any()))
            {
                stepsToProcess = stepsToProcess
                    .SelectMany(s => s.GetSectionsOutside(processed))
                    .ToList();
            }

            return stepsToProcess;
        }

        private class Step
        {
            public (int min, int max) XRange { get; init; }
            public (int min, int max) YRange { get; init; }
            public (int min, int max) ZRange { get; init; }
            public bool TurnOn { get; private init; }

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

            public bool Overlaps(Step other)
            {
                return Math.Min(XRange.max, other.XRange.max) >= Math.Max(XRange.min, other.XRange.min)
                       && Math.Min(YRange.max, other.YRange.max) >= Math.Max(YRange.min, other.YRange.min)
                       && Math.Min(ZRange.max, other.ZRange.max) >= Math.Max(ZRange.min, other.ZRange.min);
            }

            public bool IncludesPoint((int x, int y, int z) point)
            {
                return IsInRange(XRange, point.x)
                       && IsInRange(YRange, point.y)
                       && IsInRange(ZRange, point.z);
            }

            public bool IncludesStep(Step other)
            {
                return IsSubRange(XRange, other.XRange)
                       && IsSubRange(YRange, other.YRange)
                       && IsSubRange(ZRange, other.ZRange);
            }

            private bool IsSubRange((int min, int max) range, (int min, int max) subRange)
            {
                return subRange.min >= range.min && subRange.max <= range.max;
            }

            private bool IsInRange((int min, int max) range, int value)
            {
                return range.min <= value && range.max >= value;
            }

            public long GetSize()
            {
                return (long)(XRange.max - XRange.min + 1)
                       * (YRange.max - YRange.min + 1)
                       * (ZRange.max - ZRange.min + 1);
            }

            public IEnumerable<Step> GetSectionsOutside(Step other)
            {
                if (!Overlaps(other))
                {
                    return new[] { this };
                }

                return SplitX(other)
                    .SelectMany(split => split.SplitY(other))
                    .SelectMany(split => split.SplitZ(other))
                    .Where(section => !other.IncludesStep(section));
            }

            private IEnumerable<Step> SplitX(Step splitter)
            {
                if (!Overlaps(splitter))
                {
                    yield return this;
                    yield break;
                }

                foreach (var range in SplitIntoDisjointRanges(XRange, splitter.XRange))
                {
                    yield return new Step
                    {
                        XRange = range,
                        YRange = YRange,
                        ZRange = ZRange,
                        TurnOn = TurnOn
                    };
                }
            }

            private IEnumerable<Step> SplitY(Step splitter)
            {
                if (!Overlaps(splitter))
                {
                    yield return this;
                    yield break;
                }

                foreach (var range in SplitIntoDisjointRanges(YRange, splitter.YRange))
                {
                    yield return new Step
                    {
                        XRange = XRange,
                        YRange = range,
                        ZRange = ZRange,
                        TurnOn = TurnOn
                    };
                }
            }

            private IEnumerable<Step> SplitZ(Step splitter)
            {
                if (!Overlaps(splitter))
                {
                    yield return this;
                    yield break;
                }

                foreach (var range in SplitIntoDisjointRanges(ZRange, splitter.ZRange))
                {
                    yield return new Step
                    {
                        XRange = XRange,
                        YRange = YRange,
                        ZRange = range,
                        TurnOn = TurnOn
                    };
                }
            }

            private IEnumerable<(int min, int max)> SplitIntoDisjointRanges((int min, int max) inputRange, (int min, int max) splitterRange)
            {
                var remainingToSlice = inputRange;
                if (IsInRange(remainingToSlice, splitterRange.min) && splitterRange.min > remainingToSlice.min)
                {
                    yield return (remainingToSlice.min, splitterRange.min - 1);
                    remainingToSlice = (splitterRange.min, remainingToSlice.max);
                }

                if (IsInRange(remainingToSlice, splitterRange.max))
                {
                    yield return (remainingToSlice.min, splitterRange.max);
                    remainingToSlice = (splitterRange.max + 1, remainingToSlice.max);
                }

                if (remainingToSlice.max >= remainingToSlice.min)
                {
                    yield return remainingToSlice;
                }
            }
        }
    }
}