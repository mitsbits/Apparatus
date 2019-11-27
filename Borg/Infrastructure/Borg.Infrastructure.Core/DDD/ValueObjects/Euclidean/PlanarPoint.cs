using System.Collections.Generic;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    /// <summary>
    /// A data structure that represents a point on a geometric axis
    /// </summary>
    public sealed class PlanarPoint : ValueObject<PlanarPoint>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Horizontal value</param>
        /// <param name="y">Vertical value</param>
        public PlanarPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Horizontal value</param>
        /// <param name="y">Vertical value</param>
        public PlanarPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// A construcor that dupilcate a <see cref="PlanarPoint"/>
        /// </summary>
        /// <param name="source">The source <see cref="PlanarPoint"/></param>
        public PlanarPoint(PlanarPoint source) : this(source.X, source.Y)
        {
        }

        /// <summary>
        /// Horizontal value
        /// </summary>
        public double X { get; } = 0;

        /// <summary>
        /// Vertical value
        /// </summary>
        public double Y { get; } = 0;

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }

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

        /// <summary>
        /// Create a new <see cref="PlanarPoint"/> with a new horizontal value
        /// </summary>
        /// <param name="x">New horizontal value</param>
        /// <returns></returns>
        public PlanarPoint NewX(int x)
        {
            return new PlanarPoint(x, Y);
        }

        /// <summary>
        /// Create a new <see cref="PlanarPoint"/> with a new horizontal value
        /// </summary>
        /// <param name="x">New horizontal value</param>
        /// <returns></returns>
        public PlanarPoint NewX(double x)
        {
            return new PlanarPoint(x, Y);
        }

        /// <summary>
        /// Create a new <see cref="PlanarPoint"/> with a new vorizontal value
        /// </summary>
        /// <param name="y">New vorizontal value</param>
        /// <returns></returns>
        public PlanarPoint NewY(int y)
        {
            return new PlanarPoint(X, y);
        }

        /// <summary>
        /// Create a new <see cref="PlanarPoint"/> with a new vorizontal value
        /// </summary>
        /// <param name="y">New vorizontal value</param>
        /// <returns></returns>
        public PlanarPoint NewY(double y)
        {
            return new PlanarPoint(X, y);
        }

        /// <summary>
        /// Vertical value comparison
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsHigherThan(PlanarPoint other)
        {
            return Y > other.Y;
        }

        /// <summary>
        /// Vertical value comparison
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsLowerThan(PlanarPoint other)
        {
            return Y < other.Y;
        }

        /// <summary>
        /// Horizontal value comparison
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsRightThan(PlanarPoint other)
        {
            return X > other.X;
        }

        /// <summary>
        /// Horizontal value comparison
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsLeftThan(PlanarPoint other)
        {
            return X < other.X;
        }

        /// <summary>
        /// The <see cref="PlanarPoint"/> that represents the center of a geometric axis
        /// </summary>
        /// <returns></returns>
        internal static PlanarPoint Zero()
        {
            return new PlanarPoint(0, 0);
        }
    }
    /// <summary>
    /// Default sorting of points, from left to right and then from lower to higher
    /// </summary>
    internal class DefaultPointDirection : Comparer<PlanarPoint>
    {
        public override int Compare(PlanarPoint x, PlanarPoint y)
        {
            if (x.IsLeftThan(y)) return 1;
            if (x.IsRightThan(y)) return -1;
            if (x.IsLowerThan(y)) return 1;
            if (x.IsHigherThan(y)) return -1;
            return 0;
        }
    }
}