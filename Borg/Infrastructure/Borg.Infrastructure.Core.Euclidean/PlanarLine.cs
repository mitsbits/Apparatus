using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Borg.Infrastructure.Core.Euclidean
{
    public class PlanarLine : PlanarShape, IGetVector
    {
        /// <summary>
        /// the default direction comparer
        /// </summary>
        private readonly IComparer<PlanarPoint> direction = new DefaultPointDirection();

        /// <summary>
        /// Data structure to represent a line segment on a geometric axis
        /// </summary>
        /// <param name="pointOne">Line segment edge <see cref="PlanarPoint"/></param>
        /// <param name="pointTwo">Line segment edge <see cref="PlanarPoint"/></param>
        /// <param name="calculateDirection">If True will order the <see cref="PlanarPoint"/> inputs by the given <see cref="IComparable{PlanarPoint}"/> by the <see cref="direction"/> parameter or if direction is null the <see cref="DefaultPointDirection"/> </param>
        /// <param name="direction">The <see cref="IComparer{PlanarPoint}"/> to use if <see cref="calculateDirection"/> is true</param>
        public PlanarLine(PlanarPoint pointOne, PlanarPoint pointTwo, bool calculateDirection = false, IComparer<PlanarPoint> direction = null) : base(new[] { pointOne, pointTwo })
        {
            if (pointOne.Equals(pointTwo))
            {
                throw new InvalidOperationException($"this is not a line"); //TODO: create suitable exception
            }
            if (calculateDirection)
            {
                if (direction != null) this.direction = direction;
                Array.Sort(Points, this.direction);
            }
        }

        /// <summary>
        /// The starting <see cref="PlanarPoint"/>
        /// </summary>
        [ExcludeValueObjectProperty]
        public PlanarPoint PointOne => Points[0];

        /// <summary>
        /// The ending <see cref="PlanarPoint"/>
        /// </summary>
        [ExcludeValueObjectProperty]
        public PlanarPoint PointTwo => Points[1];

        public Vector2 Vector()
        {
            var substraction = PointTwo - PointOne;
            var result = new Vector2(substraction.X, substraction.Y);
            return result;
        }

        /// <summary>
        /// Calculate the angle against another <see cref="PlanarLine"/>
        /// </summary>
        /// <param name="other">The <see cref="PlanarLine"/> to define an angle</param>
        /// <returns>Degrees</returns>
        public float CornerRadius(PlanarLine other)
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
            double ALen = Math.Sqrt(pointA.X * pointA.X + pointA.Y * pointA.Y);
            double BLen = Math.Sqrt(Math.Pow(pointB.X, 2) + Math.Pow(pointB.Y, 2));
            float dotProduct = (pointA.X * pointB.X) + (pointA.Y * pointB.Y);
            float theta = (float)(180 / Math.PI) * (float)Math.Acos(dotProduct / (float)(ALen * BLen));
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
        public virtual float GetDistance()
        {
            //return (float)Math.Sqrt(GetDistanceSquared());
            return Vector2.Distance(PointOne._vector, PointTwo._vector);
        }

        /// <summary>
        /// Calculate the squared distance of the <see cref="PlanarLine"/>
        /// </summary>
        /// <returns>Squared units</returns>
        /// <remarks>Use this for comparison as it is faster that the <see cref="GetDistance"/></remarks>
        public virtual float GetDistanceSquared()
        {
            //    float xDelta = PointOne.X - PointTwo.X;
            //    float yDelta = PointOne.Y - PointTwo.Y;

            //    return (float)Math.Pow(xDelta, 2) + (float)Math.Pow(yDelta, 2);
            return Vector2.DistanceSquared(PointOne._vector, PointTwo._vector);
        }

        /// <summary>
        /// Calculate a line with the same starting point, same distance but at the given angle
        /// </summary>
        /// <param name="angle">Degrees</param>
        /// <returns>A <see cref="PlanarLine"/> a an angle</returns>
        public PlanarLine AtRadius(float angle)
        {
            var distance = GetDistance();
            var startpoint = new PlanarPoint(PointOne);
            var targetpoint = new PlanarPoint(PointOne.X + ((float)Math.Cos(angle) * distance), PointOne.Y + ((float)Math.Sin(angle) * distance));
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
        public bool IsParallelTo(PlanarLine other, float thresshold = 0.99f)
        {
            var dx1 = PointOne.X - PointTwo.X;
            var dy1 = PointTwo.Y - PointOne.Y;
            var dx2 = other.PointTwo.X - other.PointOne.X;
            var dy2 = other.PointTwo.Y - other.PointOne.Y;
            var cosAngle = (float)Math.Abs(((dx1 * dx2) + (dy1 * dy2)) / Math.Sqrt(((dx1 * dx1) + (dy1 * dy1)) * ((dx2 * dx2) + (dy2 * dy2))));
            return cosAngle > thresshold;
        }

        /// <summary>
        /// Determines the <see cref="PlanarPoint"/> that intersect, actualy or projected
        /// </summary>
        /// <param name="other"> The <see cref="PlanarPoint"/> to caclulate against</param>
        /// <returns>Will retun <see cref="null"/> if the lines do not intersect, otherwise the intersection as a <see cref="PlanarPoint"/></returns>
        public PlanarPoint Intersection(PlanarLine other)
        {
            bool lines_intersect;
            bool segments_intersect;
            PlanarPoint intersection;
            PlanarPoint close_p1;
            PlanarPoint close_p2;
            FindIntersection(other, out lines_intersect, out segments_intersect, out intersection, out close_p1, out close_p2);
            if (lines_intersect || segments_intersect) return intersection;
            return null;
        }

        private void FindIntersection(PlanarLine other, out bool lines_intersect, out bool segments_intersect, out PlanarPoint intersection, out PlanarPoint close_p1, out PlanarPoint close_p2)
        {
            // Get the segments' parameters.
            float dx12 = PointTwo.X - PointOne.X;
            float dy12 = PointTwo.Y - PointOne.Y;
            float dx34 = other.PointTwo.X - other.PointOne.X;
            float dy34 = other.PointTwo.Y - other.PointOne.Y;

            // Solve for t1 and t2
            float denominator = ((dy12 * dx34) - (dx12 * dy34));

            float t1 = (((PointOne.X - other.PointOne.X) * dy34) + ((other.PointOne.Y - PointOne.Y) * dx34)) / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = null;
                close_p1 = null;
                close_p2 = null;
                return;
            }
            lines_intersect = true;

            float t2 =
                (((other.PointOne.X - PointOne.X) * dy12) + ((PointOne.Y - other.PointOne.Y) * dx12)) / -denominator;

            // Find the point of intersection.
            intersection = new PlanarPoint(PointOne.X + dx12 * t1, PointOne.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new PlanarPoint(PointOne.X + (dx12 * t1), PointOne.Y + (dy12 * t1));
            close_p2 = new PlanarPoint(other.PointOne.X + (dx34 * t2), other.PointOne.Y + (dy34 * t2));
        }
    }
}