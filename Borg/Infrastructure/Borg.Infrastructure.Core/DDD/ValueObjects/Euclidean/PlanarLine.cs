using System;
using System.Linq;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarLine : PlanarShape
    {
        /// <summary>
        /// Data structure to represent a line on a geometric axis
        /// </summary>
        /// <param name="pointOne">Start</param>
        /// <param name="pointTwo">End</param>
        public PlanarLine(PlanarPoint pointOne, PlanarPoint pointTwo) : base(new[] { pointOne, pointTwo })
        {
            if (pointOne.Equals(pointTwo))
            {
                throw new InvalidOperationException($"this is not a line"); //TODO: create suitable exception
            }
        }

        /// <summary>
        /// The starting <see cref="PlanarPoint"/>
        /// </summary>
        public PlanarPoint PointOne => Points[0];

        /// <summary>
        /// The ending <see cref="PlanarPoint"/>
        /// </summary>
        public PlanarPoint PointTwo => Points[1];

        /// <summary>
        /// Calculate the angle against another <see cref="PlanarLine"/>
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to define an angle</param>
        /// <returns>Degrees</returns>
        public double CornerRadius(PlanarLine other)
        {
            //var x1 = PointOne.X;
            //var y1 = PointOne.Y;
            //var x2 = PointTwo.X;
            //var y2 = PointTwo.Y;
            //var x3 = other.PointOne.X;
            //var y3 = other.PointOne.Y;
            //var x4 = other.PointTwo.X;
            //var y4 = other.PointTwo.Y;

            //var angle = Math.Atan2(y2 - y1, x2 - x1) - Math.Atan2(y4 - y3, x4 - x3);
            //return angle;
            var point1 = PointOne;
            var point2 = PointTwo;
            var point3 = other.Points.FirstOrDefault(x => x != point1 && x != point2);
            if (point3 == null) return 0;
            var pointA = point1 - point2;
            var pointB = point3 - point2;
            double ALen = Math.Sqrt(Math.Pow(pointA.X, 2) + Math.Pow(pointA.Y, 2));
            double BLen = Math.Sqrt(Math.Pow(pointB.X, 2) + Math.Pow(pointB.Y, 2));
            double dotProduct = (pointA.X * pointB.X) + (pointA.Y * pointB.Y);
            double theta = (180 / Math.PI) * Math.Acos(dotProduct / (ALen * BLen));
            return theta;
        }

        public override PlanarPoint Center()
        {
            return new PlanarPoint((PointOne.X + PointTwo.X) / 2, (PointOne.Y + PointTwo.Y) / 2);
        }

        /// <summary>
        /// Calculate the distance of the <see cref="PlanarLine"/>
        /// </summary>
        /// <returns>Units</returns>
        public virtual double GetDistance()
        {
            return Math.Sqrt(GetDistanceSquared());
        }

        /// <summary>
        /// Calculate the squared distance of the <see cref="PlanarLine"/>
        /// </summary>
        /// <returns>Squared units</returns>
        /// <remarks>Use this for comparison as it is faster that the <see cref="GetDistance"/></remarks>
        public virtual double GetDistanceSquared()
        {
            double xDelta = PointOne.X - PointTwo.X;
            double yDelta = PointOne.Y - PointTwo.Y;

            return Math.Pow(xDelta, 2) + Math.Pow(yDelta, 2);
        }

        /// <summary>
        /// Calculate a line with the same starting point, same distance but at the given angle
        /// </summary>
        /// <param name="angle">Degrees</param>
        /// <returns>A <see cref="PlanarLine"/> a an angle</returns>
        public PlanarLine AtRadius(double angle)
        {
            var distance = GetDistance();
            var startpoint = new PlanarPoint(PointOne);
            var targetpoint = new PlanarPoint(PointOne.X + (Math.Cos(angle) * distance), PointOne.Y + (Math.Sin(angle) * distance));
            return new PlanarLine(startpoint, targetpoint);
        }

        /// <summary>
        /// Check if this line is longer than the given <see cref="PlanarLine"/>
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to compare</param>
        /// <returns></returns>
        public bool IsLongerThan(PlanarLine other)
        {
            return GetDistanceSquared() > other.GetDistanceSquared();
        }

        /// <summary>
        /// Check if this line is shorter than the given <see cref="PlanarLine"/>
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to compare</param>
        /// <returns></returns>
        public bool IsShorterThan(PlanarLine other)
        {
            return GetDistanceSquared() > other.GetDistanceSquared();
        }

        /// <summary>
        /// Check if this line has the same distance as the given <see cref="PlanarLine"/>
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to compare</param>
        /// <returns></returns>
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