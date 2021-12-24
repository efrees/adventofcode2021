using System;

namespace AdventOfCode2021.Grid
{
    public record Point3D(long X, long Y, long Z)
    {
        public static Point3D Origin { get; } = new(0, 0, 0);

        public static implicit operator (long x, long y, long z)(Point3D point) => (x: point.X, y: point.Y, z: point.Z);
        public static implicit operator Point3D((long x, long y, long z) coordinates) => new(coordinates.x, coordinates.y, coordinates.z);
        public static implicit operator Point3D((int x, int y, long z) coordinates) => new(coordinates.x, coordinates.y, coordinates.z);

        public Point3D Add(Point3D other)
        {
            return new(X + other.X, Y + other.Y, Z + other.Z);
        }

        public Point3D Subtract(Point3D other)
        {
            return new(X - other.X, Y - other.Y, Z - other.Z);
        }

        public long ManhattanDistance(Point3D other)
        {
            return Math.Abs(X - other.X)
                   + Math.Abs(Y - other.Y)
                   + Math.Abs(Z - other.Z);
        }

        public Point3D Rotate90AroundX()
        {
            return new(X, -Z, Y);
        }

        public Point3D Rotate90AroundY()
        {
            return new(Z, Y, -X);
        }

        public Point3D Rotate90AroundZ()
        {
            return new(-Y, X, Z);
        }
    }
}