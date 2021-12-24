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
            var scanners = ParseAll(Input.GetLinesFromFile(InputFile))
                .ToList();
            var alignedScanners = AlignScanners(scanners);

            Console.WriteLine($"Output (part 1): {GetPart1Answer(alignedScanners)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(alignedScanners)}");
        }

        private static long GetPart1Answer(List<ScannerData> alignedScanners)
        {
            return alignedScanners
                .SelectMany(scanner => scanner.BeaconPositions)
                .Distinct()
                .Count();
        }

        private static long GetPart2Answer(List<ScannerData> alignedScanners)
        {
            var alignedScannerPositions = alignedScanners
                .Select(scanner => scanner.RelativeScannerPosition)
                .ToList();

            return alignedScannerPositions.CrossProduct(alignedScannerPositions)
                .Select(pair => pair.Item1.ManhattanDistance(pair.Item2))
                .Max();
        }

        private static List<ScannerData> AlignScanners(List<ScannerData> input)
        {
            var alignedScanners = new List<ScannerData> { input.First() };
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

            return alignedScanners;
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
            public Point3D RelativeScannerPosition { get; private set; } = Point3D.Origin;

            public bool TryAlignTo(ScannerData alignedScanner)
            {
                var rotationPath = new[] { 'x', 'z', 'x', 'z', 'x', 'z' };
                foreach (var topFaceRotation in rotationPath)
                {
                    if (topFaceRotation == 'x')
                    {
                        BeaconPositions = BeaconPositions.Select(point => point.Rotate90AroundX()).ToList();
                        RelativeScannerPosition = RelativeScannerPosition.Rotate90AroundX();
                    }
                    else if (topFaceRotation == 'z')
                    {
                        BeaconPositions = BeaconPositions.Select(point => point.Rotate90AroundZ()).ToList();
                        RelativeScannerPosition = RelativeScannerPosition.Rotate90AroundZ();
                    }

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
                                    RelativeScannerPosition = RelativeScannerPosition.Add(shiftAmount);
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