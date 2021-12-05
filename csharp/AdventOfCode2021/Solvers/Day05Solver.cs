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
            var intersections = FillSegmentsOnGrid(segments.Where(s => s.IsVertical || s.IsHorizontal));

            return intersections.Values.Count(v => v >= 2);
        }

        private static long GetPart2Answer(List<LineSegment> segments)
        {
            var intersections = FillSegmentsOnGrid(segments);

            return intersections.Values.Count(v => v >= 2);
        }

        private static Dictionary<(int, int), int> FillSegmentsOnGrid(IEnumerable<LineSegment> segments)
        {
            var intersections = new Dictionary<(int, int), int>();

            foreach (var segment in segments)
            {
                if (segment.IsHorizontal)
                {
                    var y = segment.Start.y;
                    var xMin = Math.Min(segment.Start.x, segment.End.x);
                    var xMax = Math.Max(segment.Start.x, segment.End.x);
                    for (var x = xMin; x <= xMax; x++)
                    {
                        intersections[(x, y)] = intersections.GetValueOrDefault((x, y), 0) + 1;
                    }
                }
                else if (segment.IsVertical)
                {
                    var x = segment.Start.x;
                    var yMin = Math.Min(segment.Start.y, segment.End.y);
                    var yMax = Math.Max(segment.Start.y, segment.End.y);
                    for (var y = yMin; y <= yMax; y++)
                    {
                        intersections[(x, y)] = intersections.GetValueOrDefault((x, y), 0) + 1;
                    }
                }
                else if (segment.IsUpDiagonal)
                {
                    var xMin = Math.Min(segment.Start.x, segment.End.x);
                    var xMax = Math.Max(segment.Start.x, segment.End.x);
                    var y = Math.Min(segment.Start.y, segment.End.y);
                    for (var x = xMin; x <= xMax; x++, y++)
                    {
                        intersections[(x, y)] = intersections.GetValueOrDefault((x, y), 0) + 1;
                    }
                }
                else if (segment.IsDownDiagonal)
                {
                    var xMin = Math.Min(segment.Start.x, segment.End.x);
                    var xMax = Math.Max(segment.Start.x, segment.End.x);
                    var y = Math.Max(segment.Start.y, segment.End.y);
                    for (var x = xMin; x <= xMax; x++, y--)
                    {
                        intersections[(x, y)] = intersections.GetValueOrDefault((x, y), 0) + 1;
                    }
                }
            }

            return intersections;
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
            public (int x, int y) Start { get; set; }
            public (int x, int y) End { get; set; }

            public bool IsHorizontal => Start.y == End.y;
            public bool IsVertical => Start.x == End.x;
            public bool IsUpDiagonal => Start.x - End.x == Start.y - End.y;
            public bool IsDownDiagonal => Start.x - End.x == -(Start.y - End.y);
        }
    }
}