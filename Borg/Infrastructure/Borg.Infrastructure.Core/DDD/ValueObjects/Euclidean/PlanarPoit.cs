namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarPoint : ValueObject<PlanarPoint>
    {
        public PlanarPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public PlanarPoint(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public decimal X { get; }
        public decimal Y { get; }

        public PlanarPoint NewX(int x)
        {
            return new PlanarPoint(x, Y);
        }

        public PlanarPoint NewX(decimal x)
        {
            return new PlanarPoint(x, Y);
        }

        public PlanarPoint NewY(int y)
        {
            return new PlanarPoint(X, y);
        }

        public PlanarPoint NewY(decimal y)
        {
            return new PlanarPoint(X, y);
        }

        public bool IsHigherThan(PlanarPoint other)
        {
            return Y > other.Y;
        }

        public bool IsLowerThan(PlanarPoint other)
        {
            return Y < other.Y;
        }

        public bool IsRightThan(PlanarPoint other)
        {
            return X > other.X;
        }

        public bool IsLeftThan(PlanarPoint other)
        {
            return X < other.X;
        }
    }
}