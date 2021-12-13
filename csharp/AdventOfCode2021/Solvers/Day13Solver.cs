using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day13Solver : ISolver
    {
        private const string Name = "Day 13";

        private const string InputFile = "day13input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private int GetPart1Answer(List<string> lines)
        {
            var (points, folds) = ParseInput(lines);

            points = FoldAt(folds.First(), points);
            return points.Count();
        }

        private string GetPart2Answer(List<string> lines)
        {
            var (points, folds) = ParseInput(lines);

            foreach (var fold in folds)
            {
                points = FoldAt(fold, points);
            }

            return "\n" + PrintAsGrid(points);
        }

        private IEnumerable<(int x, int y)> FoldAt(Fold fold, IEnumerable<(int x, int y)> points)
        {
            return points.Select(point => fold.Dimension == 'x'
                    ? (x: ReflectValueAbove(fold.Location, point.x), point.y)
                    : (point.x, y: ReflectValueAbove(fold.Location, point.y)))
                .Distinct();
        }

        private int ReflectValueAbove(int reference, int pointValue)
        {
            return pointValue > reference
                ? reference * 2 - pointValue
                : pointValue;
        }

        private (IEnumerable<(int x, int y)> points, IList<Fold> folds) ParseInput(List<string> lines)
        {
            var points = new List<(int x, int y)>();
            var folds = new List<Fold>();

            var finishedPoints = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    finishedPoints = true;
                    continue;
                }

                if (finishedPoints)
                {
                    folds.Add(new Fold
                    {
                        Dimension = line["fold along ".Length],
                        Location = int.Parse(line["fold along x=".Length..])
                    });
                }
                else
                {
                    var coords = line.Split(',').Select(int.Parse).ToArray();
                    points.Add((coords[0], coords[1]));
                }
            }

            return (points, folds);
        }

        private string PrintAsGrid(IEnumerable<(int x, int y)> points)
        {
            var maxX = points.Select(p => p.x).Max();
            var maxY = points.Select(p => p.y).Max();
            var visible = points.ToHashSet();

            return string.Join("\n", Enumerable.Range(0, maxY + 1).Select(y =>
            {
                return Enumerable.Range(0, maxX + 1).Select(x => visible.Contains((x, y))
                    ? '#'
                    : '.');
            }).Select(chars => new string(chars.ToArray())));
        }

        private class Fold
        {
            public char Dimension { get; set; }
            public int Location { get; set; }
        }
    }
}