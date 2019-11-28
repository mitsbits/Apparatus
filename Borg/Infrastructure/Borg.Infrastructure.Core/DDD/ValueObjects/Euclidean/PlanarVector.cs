using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarVector : ValueObject<PlanarVector>
    {
        internal PlanarVector(double direction, double magnitude)
        {
            Direction = direction;
            Magnitude = magnitude;
        }

        public double Direction { get; }
        public double Magnitude { get; }

        public double DotProduct(PlanarVector other) => (Direction * other.Direction) + (Magnitude * other.Magnitude);

        public double DistanceSquared() => (Direction * Direction) + (Magnitude * Magnitude);

        public double Distance() => Math.Sqrt(DistanceSquared());

        public static PlanarVector operator +(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a.Direction + b.Direction, a.Magnitude + b.Magnitude);
        }

        public static PlanarVector operator -(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a.Direction - b.Direction, a.Magnitude - b.Magnitude);
        }
    }
}