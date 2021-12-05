using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Solvers
{
    internal class Day05Solver : ISolver
    {
        private const string Name = "Day 5";

        private const string InputFile = "day05input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile).Select(ParseLine).ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static long GetPart1Answer(List<LineSegment> segments)
        {
            return CountIntersections(segments.Where(s => s.IsVertical || s.IsHorizontal));
        }

        private static long GetPart2Answer(List<LineSegment> segments)
        {
            return CountIntersections(segments);
        }

        private static int CountIntersections(IEnumerable<LineSegment> segments)
        {
            return segments.SelectMany(s => s.AsPointSequence())
                .GroupBy(point => point)
                .Count(group => group.Count() > 1);
        }

        private LineSegment ParseLine(string line)
        {
            var groups = Regex.Match(line, "(\\d+),(\\d+) -> (\\d+),(\\d+)").Groups;
            return new LineSegment
            {
                Start = (int.Parse(groups[1].Value), int.Parse(groups[2].Value)),
                End = (int.Parse(groups[3].Value), int.Parse(groups[4].Value))
            };
        }

        private class LineSegment
        {
            public (int x, int y) Start { get; init; }
            public (int x, int y) End { get; init; }

            public bool IsHorizontal => Start.y == End.y;
            public bool IsVertical => Start.x == End.x;

            public IEnumerable<(int x, int y)> AsPointSequence()
            {
                var xStep = NormalizeStep(End.x - Start.x);
                var yStep = NormalizeStep(End.y - Start.y);

                for (var point = Start; point != End; point = (point.x + xStep, point.y + yStep))
                {
                    yield return point;
                }
            }

            private static int NormalizeStep(int totalTravel)
            {
                return totalTravel switch
                {
                    < 0 => -1,
                    0 => 0,
                    > 0 => 1
                };
            }
        }
    }
}