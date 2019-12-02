using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public struct PlanarVector 
    {
        internal PlanarVector(double magnitude, double direction)
        {
            Magnitude = magnitude;
            Direction = direction;

        }

        public double Direction { get; }
        public double Magnitude { get; }

        public double DotProduct(PlanarVector other) => (Direction * other.Direction) + (Magnitude * other.Magnitude);

        public double DistanceSquared() => (Direction * Direction) + (Magnitude * Magnitude);

        public double Distance() => Math.Sqrt(DistanceSquared());

        public static PlanarVector operator +(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a.Magnitude + b.Magnitude, a.Direction + b.Direction);
        }

        public static PlanarVector operator -(PlanarVector a, PlanarVector b)
        {
            return new PlanarVector(a.Magnitude - b.Magnitude, a.Direction - b.Direction);
        }
    }
}