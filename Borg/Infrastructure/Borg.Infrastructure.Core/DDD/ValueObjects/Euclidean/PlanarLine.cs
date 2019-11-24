using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarLine : PlanarShape
    {
        public PlanarLine(PlanarPoint pointOne, PlanarPoint pointTwo) :base(new[] { pointOne, pointTwo })
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

        public PlanarPoint Center()
        {
            return new PlanarPoint((PointOne.X + PointTwo.X) / 2, (PointOne.Y + PointTwo.Y) / 2);
        }
        //TODO: this is slow, try warking out number squares
        public virtual double GetDistance()
        {
            double xDelta = PointOne.X - PointTwo.X;
            double yDelta = PointOne.Y - PointTwo.Y;

            return Math.Sqrt(Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2));
        }

        public PlanarLine AtRadius(double angle)
        {
            var distance = GetDistance();
            var startpoint = new PlanarPoint(PointOne);
            var targetpoint = new PlanarPoint(PointOne.X + Math.Cos(angle) * distance, PointOne.Y + Math.Sin(angle) * distance);
            return new PlanarLine(startpoint, targetpoint);

        }
    }
}