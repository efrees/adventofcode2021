using AdventOfCode2021.Grid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day19Solver : ISolver
    {
        private const string Name = "Day 19";

        private const string InputFile = "day19input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = ParseAll(Input.GetLinesFromFile(InputFile))
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            //Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<ScannerData> input)
        {
            // start by calling first scanner "aligned"
            var alignedScanners = new List<ScannerData> { input.First() };
            input.RemoveAt(0);
            for (var i = 0; i < alignedScanners.Count; i++)
            {
                var alignedScanner = alignedScanners[i];
                foreach (var unalignedScanner in input.Except(alignedScanners))
                {
                    if (unalignedScanner.TryAlignTo(alignedScanner))
                    {
                        alignedScanners.Add(unalignedScanner);
                    }
                }
            }

            var uniquePoints = alignedScanners.SelectMany(scanner => scanner.BeaconPositions).ToHashSet();
            return uniquePoints.Count;
        }

        private static IEnumerable<ScannerData> ParseAll(IEnumerable<string> lines)
        {
            var nextScanner = new ScannerData();
            var linesWithoutFirstHeader = lines.Skip(1);
            foreach (var line in linesWithoutFirstHeader.Where(line => !string.IsNullOrWhiteSpace(line)))
            {
                if (line.StartsWith("---"))
                {
                    yield return nextScanner;
                    nextScanner = new ScannerData();
                    continue;
                }

                var coordinates = line.Split(",").Select(int.Parse).ToArray();
                nextScanner.BeaconPositions.Add((coordinates[0], coordinates[1], coordinates[2]));
            }

            yield return nextScanner;
        }

        private class ScannerData
        {
            public List<Point3D> BeaconPositions { get; private set; } = new();

            public bool TryAlignTo(ScannerData alignedScanner)
            {
                var rotationPath = new[] { 'x', 'z', 'x', 'z', 'x', 'z' };
                foreach (var topFaceRotation in rotationPath)
                {
                    BeaconPositions = topFaceRotation switch
                    {
                        'x' => BeaconPositions.Select(point => point.Rotate90AroundX()).ToList(),
                        'z' => BeaconPositions.Select(point => point.Rotate90AroundZ()).ToList(),
                    };

                    foreach (var _ in Enumerable.Range(1, 4))
                    {
                        BeaconPositions = BeaconPositions.Select(point => point.Rotate90AroundY()).ToList();

                        var alignedPoints = alignedScanner.BeaconPositions.ToHashSet();
                        foreach (var referencePoint in alignedPoints)
                        {
                            for (var i = 0; i < BeaconPositions.Count - 11; i++)
                            {
                                // Try this one as a match for the reference
                                var matchingPoint = BeaconPositions[i];
                                var shiftAmount = referencePoint.Subtract(matchingPoint);
                                var shiftedPoints = BeaconPositions.Skip(i).Select(point => point.Add(shiftAmount));
                                if (shiftedPoints.Count(point => alignedPoints.Contains(point)) >= 12)
                                {
                                    BeaconPositions = BeaconPositions.Select(point => point.Add(shiftAmount)).ToList();
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}