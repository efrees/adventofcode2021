using System;
using System.Collections.Generic;

namespace AdventOfCode2021.Grid
{
    public record Point2D(long X, long Y)
    {
        public static Point2D Origin { get; } = new(0, 0);

        public static implicit operator (long x, long y)(Point2D point) => (x: point.X, y: point.Y);
        public static implicit operator Point2D((long x, long y) coordinates) => new(coordinates.x, coordinates.y);
        public static implicit operator Point2D((int x, int y) coordinates) => new(coordinates.x, coordinates.y);

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

        public Point2D Add(Point2D other)
        {
            return new(X + other.X, Y + other.Y);
        }

        public Point2D Subtract(Point2D other)
        {
            return new(X - other.X, Y - other.Y);
        }
        public long ManhattanDistance(Point2D other)
        {
            return Math.Abs(X - other.X)
                   + Math.Abs(Y - other.Y);
        }
    }
}