namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public partial class PlanarPoint : ValueObject<PlanarPoint>
    {
        public PlanarPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public PlanarPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public PlanarPoint(PlanarPoint source) : this(source.X, source.Y)
        {
        }

        public double X { get; }
        public double Y { get; }
        public static PlanarPoint operator +(PlanarPoint a, PlanarPoint b)
        {
            return new PlanarPoint(a.X + b.X, a.Y + b.Y);
        }

        public static PlanarPoint operator -(PlanarPoint a, PlanarPoint b)
        {
            return new PlanarPoint(a.X - b.X, a.Y - b.Y);
        }

        public static PlanarPoint operator -(PlanarPoint a)
        {
            return Zero() - a;
        }

        public PlanarPoint NewX(int x)
        {
            return new PlanarPoint(x, Y);
        }

        public PlanarPoint NewX(double x)
        {
            return new PlanarPoint(x, Y);
        }

        public PlanarPoint NewY(int y)
        {
            return new PlanarPoint(X, y);
        }

        public PlanarPoint NewY(double y)
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


        internal static PlanarPoint Zero()
        {
            return new PlanarPoint(0, 0);
        }

    }
}