using System.Collections.Generic;

namespace AdventOfCode2021.Grid
{
    public record Point2D(long X, long Y)
    {
        public static implicit operator (long x, long y)(Point2D point) => (x: point.X, y: point.Y);

        public IEnumerable<Point2D> GetNeighbors4()
        {
            return new[]
            {
                new Point2D(X - 1, Y),
                new Point2D(X, Y - 1),
                new Point2D(X + 1, Y),
                new Point2D(X, Y + 1),
            };
        }
    }
}