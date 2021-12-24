using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2021.Grid
{
    public class SparseGrid<TCell>
    {
        private readonly Dictionary<(long x, long y), TCell> _data = new();
        public long xMin {get; private set;} = long.MaxValue;
        public long xMax {get; private set;} = long.MinValue;
        public long yMin {get; private set;} = long.MaxValue;
        public long yMax {get; private set; } = long.MinValue;

        public static SparseGrid<TCell> Parse(IEnumerable<string> input, Func<char, TCell> valueSelector)
        {
            var grid = new SparseGrid<TCell>();
            var allPairs = input
                .SelectMany((line, y) => line.Select((ch, x) => (coords: (x, y), val: valueSelector(ch))));
            foreach (var (coords, val) in allPairs)
            {
                grid.SetCell(coords, val);
            }

            return grid;
        }

        public IEnumerable<Point2D> GetAllCoordinates()
        {
            return _data.Keys.Select(k => (Point2D)k);
        }

        public void SetCell(Point2D coordinates, TCell value)
        {
            xMin = Math.Min(xMin, coordinates.X);
            xMax = Math.Max(xMax, coordinates.X);
            yMin = Math.Min(yMin, coordinates.Y);
            yMax = Math.Max(yMax, coordinates.Y);
            _data[coordinates] = value;
        }

        public TCell GetCell(Point2D coordinates, TCell defaultValue = default)
        {
            return _data.GetValueOrDefault(coordinates, defaultValue);
        }

        /// <summary>
        /// Only use this if the grid is expected to be fairly small.
        /// </summary>
        /// <param name="renderFunc"></param>
        /// <returns></returns>
        public string RenderAsString(Func<TCell, char> renderFunc)
        {
            var sb = new StringBuilder();
            for (var i = yMin; i <= yMax; i++)
            {
                for (var j = xMin; j <= xMax; j++)
                {
                    sb.Append(renderFunc(_data.GetValueOrDefault((j, i))));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}