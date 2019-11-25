using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarLine : PlanarShape
    {
        public PlanarLine(PlanarPoint pointOne, PlanarPoint pointTwo) : base(new[] { pointOne, pointTwo })
        {
            if (pointOne.Equals(pointTwo))
            {
                throw new InvalidOperationException($"this is not a line"); //TODO: create suitable exception
            }
        }

        public PlanarPoint PointOne => Points[0];
        public PlanarPoint PointTwo => Points[1];

        public double CornerRadius(PlanarLine other)
        {
            var x1 = PointOne.X;
            var y1 = PointOne.Y;
            var x2 = PointTwo.X;
            var y2 = PointTwo.Y;
            var x3 = other.PointOne.X;
            var y3 = other.PointOne.Y;
            var x4 = other.PointTwo.X;
            var y4 = other.PointTwo.Y;

            var angle = Math.Atan2(y2 - y1, x2 - x1) - Math.Atan2(y4 - y3, x4 - x3);
            return angle;
        }

        public override PlanarPoint Center()
        {
            return new PlanarPoint((PointOne.X + PointTwo.X) / 2, (PointOne.Y + PointTwo.Y) / 2);
        }

        //TODO: this is slow, try warking out number squares
        public virtual double GetDistance()
        {
            return Math.Sqrt(GetDistanceSquared());
        }

        public virtual double GetDistanceSquared()
        {
            double xDelta = PointOne.X - PointTwo.X;
            double yDelta = PointOne.Y - PointTwo.Y;

            return Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2);
        }

        public PlanarLine AtRadius(double angle)
        {
            var distance = GetDistance();
            var startpoint = new PlanarPoint(PointOne);
            var targetpoint = new PlanarPoint(PointOne.X + Math.Cos(angle) * distance, PointOne.Y + Math.Sin(angle) * distance);
            return new PlanarLine(startpoint, targetpoint);
        }

        public bool IsLongerThan(PlanarLine other)
        {
            return GetDistanceSquared() > other.GetDistanceSquared();
        }

        public bool IsShorterThan(PlanarLine other)
        {
            return GetDistanceSquared() > other.GetDistanceSquared();
        }

        public bool IsSameDistance(PlanarLine other)
        {
            return GetDistanceSquared() == other.GetDistanceSquared();
        }

        /// <summary>
        /// Determine if a line is parallel to an other on a error threshold
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to compare</param>
        /// <param name="thresshold">10° are about 0.9848 threshold value</param>
        /// <returns><see cref="Boolean"/>True if the lines are parallel</returns>
        public bool IsParallelTo(PlanarLine other, double thresshold = 0.99)
        {
            var dx1 = PointOne.X - PointTwo.X;
            var dy1 = PointTwo.Y - PointOne.Y;
            var dx2 = other.PointTwo.X - other.PointOne.X;
            var dy2 = other.PointTwo.Y - other.PointOne.Y;
            var cosAngle = Math.Abs(((dx1 * dx2) + (dy1 * dy2)) / Math.Sqrt(((dx1 * dx1) + (dy1 * dy1)) * ((dx2 * dx2) + (dy2 * dy2))));
            return cosAngle > thresshold;
        }
    }
}