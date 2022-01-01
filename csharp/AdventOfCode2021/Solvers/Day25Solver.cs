using AdventOfCode2021.Grid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day25Solver : ISolver
    {
        private const string Name = "Day 25";

        private const string InputFile = "day25input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
        }

        private int GetPart1Answer(List<string> lines)
        {
            var height = lines.Count;
            var width = lines[0].Length;
            var grid = SparseGrid<char>.Parse(lines, x => x);
            var step = 0;
            var hasChanged = true;
            while (hasChanged)
            {
                var nextGrid = new SparseGrid<char>();
                hasChanged = false;
                foreach (var coords in grid.GetAllCoordinates())
                {
                    if (grid.GetCell(coords) == '>')
                    {
                        var target = ((coords.X + 1) % width, coords.Y);
                        if (grid.GetCell(target) == '.' || grid.GetCell(target) == 0)
                        {
                            nextGrid.SetCell(target, '>');
                            hasChanged = true;
                        }
                        else
                        {
                            nextGrid.SetCell(coords, '>');
                        }
                    }
                }
                foreach (var coords in grid.GetAllCoordinates())
                {
                    if (grid.GetCell(coords) == 'v')
                    {
                        var target = (coords.X, (coords.Y + 1) % height);
                        if (nextGrid.GetCell(target) != '>' && grid.GetCell(target) != 'v')
                        {
                            nextGrid.SetCell(target, 'v');
                            hasChanged = true;
                        }
                        else
                        {
                            nextGrid.SetCell(coords, 'v');
                        }
                    }
                }

                grid = nextGrid;
                step++;
            }

            return step;
        }
    }
}