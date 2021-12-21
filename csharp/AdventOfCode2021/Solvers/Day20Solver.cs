using AdventOfCode2021.Grid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Solvers
{
    internal class Day20Solver : ISolver
    {
        private const string Name = "Day 20";

        private const string InputFile = "day20input.txt";

        public void Solve()
        {
            Console.WriteLine(Name);
            var lines = Input.GetLinesFromFile(InputFile)
                .ToList();

            Console.WriteLine($"Output (part 1): {GetPart1Answer(lines)}");
            Console.WriteLine($"Output (part 2): {GetPart2Answer(lines)}");
        }

        private static int GetPart1Answer(List<string> input)
        {
            var algorithm = input.First();
            var defaultChar = '.';

            var pictureRows = input.Skip(2);
            var grid = SparseGrid<char>.Parse(pictureRows, x => x);
            var nextGrid = new SparseGrid<char>();
            RunAlgorithm(algorithm, grid, nextGrid, defaultChar);
            (grid, nextGrid) = (nextGrid, grid);
            defaultChar = algorithm[0];
            RunAlgorithm(algorithm, grid, nextGrid, defaultChar);

            return nextGrid.GetAllCoordinates().Count(coord => nextGrid.GetCell(coord) == '#');
        }

        private static int GetPart2Answer(List<string> input)
        {
            var algorithm = input.First();
            var defaultChar = '.';

            var pictureRows = input.Skip(2);
            var grid = SparseGrid<char>.Parse(pictureRows, x => x);
            var nextGrid = new SparseGrid<char>();
            foreach (var _ in Enumerable.Range(1, 50))
            {
                RunAlgorithm(algorithm, grid, nextGrid, defaultChar);
                defaultChar = defaultChar switch
                {
                    '.' => algorithm[0],
                    '#' => algorithm.Last(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                (grid, nextGrid) = (nextGrid, grid);
            }

            return grid.GetAllCoordinates().Count(coord => grid.GetCell(coord) == '#');
        }

        private static void RunAlgorithm(string algorithm, SparseGrid<char> grid, SparseGrid<char> nextGrid, char defaultChar)
        {
            for (var i = grid.yMin - 2; i <= grid.yMax + 2; i++)
            {
                for (var j = grid.xMin - 2; j <= grid.xMax + 2; j++)
                {
                    var neighborhood = GetBinaryNeighborhood(grid, (j, i), defaultChar);
                    nextGrid.SetCell((j, i), algorithm[neighborhood]);
                }
            }
        }

        private static int GetBinaryNeighborhood(SparseGrid<char> grid, Point2D point, char defaultChar)
        {
            var value = 0;
            foreach (var i in Enumerable.Range(-1, 3))
            {
                foreach (var j in Enumerable.Range(-1, 3))
                {
                    value *= 2;
                    if (grid.GetCell(point.Add((j, i)), defaultChar) == '#')
                    {
                        value += 1;
                    }
                }
            }

            return value;
        }
    }
}