using Borg.Infrastructure.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Borg.Infrastructure.Core.Euclidean
{
    /// <summary>
    /// Abstract data structure representing a shape on a geometric axis
    /// </summary>
    public abstract class PlanarShape : ValueObject<PlanarShape>
    {
        /// <summary>
        ///Constructor
        /// </summary>
        /// <param name="points"></param>
        protected PlanarShape(IEnumerable<PlanarPoint> points)
        {
            Points = ValidatePoints(Preconditions.NotEmpty(points, nameof(points)));
        }

        /// <summary>
        /// Collection of distinct point that define the <see cref="PlanarShape"/>
        /// </summary>
        public PlanarPoint[] Points { get; private set; }

        /// <summary>
        /// Validation of the dource collection od the source points
        /// Each derived class should override the method
        /// </summary>
        /// <param name="points">Source points</param>
        /// <returns>Distinct array of points</returns>
        protected virtual PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() < 2) throw new InvalidOperationException($"At least two points are required for a {nameof(PlanarShape)}");
            return distinctPoints.ToArray();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var point in Points)
            {
                builder.Append(point.ToString());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Detrmine the <see cref="PlanarPoint"/> int the center of the <see cref="PlanarShape"/>
        /// </summary>
        /// <returns></returns>
        public virtual PlanarPoint Center()
        {
            return CenterInternal();
        }

        private float FurtherPointsVlaue(Compass orientation)
        {
            float result;
            switch (orientation)
            {
                case Compass.North:
                    result = Points.Max(x => x.Y);
                    break;

                case Compass.East:
                    result = Points.Max(x => x.X);
                    break;

                case Compass.South:
                    result = Points.Min(x => x.Y);
                    break;

                case Compass.West:
                    result = Points.Min(x => x.X);
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"{orientation.ToString()} is not valid {nameof(Compass)} value");
                    break;
            }
            return result;
        }

        private PlanarPoint Lower()
        {
            var eligibles = Points.Where(x => x.Y == FurtherPointsVlaue(Compass.South));
            if (eligibles.Count() == 1) return eligibles.Single();
            return eligibles.OrderBy(x => x.X).First();
        }

        private PlanarPoint Higher()
        {
            var eligibles = Points.Where(x => x.Y == FurtherPointsVlaue(Compass.North));
            if (eligibles.Count() == 1) return eligibles.Single();
            return eligibles.OrderBy(x => x.X).First();
        }

        private PlanarPoint Lefter()
        {
            var eligibles = Points.Where(x => x.X == FurtherPointsVlaue(Compass.West));
            if (eligibles.Count() == 1) return eligibles.Single();
            return eligibles.OrderBy(x => x.Y).First();
        }

        private PlanarPoint Righter()
        {
            var eligibles = Points.Where(x => x.X == FurtherPointsVlaue(Compass.East));
            if (eligibles.Count() == 1) return eligibles.Single();
            return eligibles.OrderBy(x => x.Y).First();
        }

        private PlanarPoint CenterInternal()
        {
            int collectionCount = Points.Length;
            var localCollection = new PlanarPoint[collectionCount + 1];
            float localX = 0, localY = 0;
            Points.CopyTo(localCollection, 0);
            localCollection[collectionCount] = Points[0];
            float factor;

            for (int i = 0; i < collectionCount; i++)
            {
                factor =
                    localCollection[i].X * localCollection[i + 1].Y -
                    localCollection[i + 1].X * localCollection[i].Y;
                localX += (localCollection[i].X + localCollection[i + 1].X) * factor;
                localY += (localCollection[i].Y + localCollection[i + 1].Y) * factor;
            }

            float polygon_area = ShapeArea();
            localX /= (6 * polygon_area);
            localY /= (6 * polygon_area);

            return localX < 0 ? new PlanarPoint(-localX, -localY) : new PlanarPoint(localX, localY);
        }

        private float SignedShapeArea()
        {
            int collectionCount = Points.Length;
            PlanarPoint[] localCollection = new PlanarPoint[collectionCount + 1];
            Points.CopyTo(localCollection, 0);
            localCollection[collectionCount] = Points[0];

            float area = 0;
            for (int i = 0; i < collectionCount; i++)
            {
                area += (localCollection[i + 1].X - localCollection[i].X) *
                        (localCollection[i + 1].Y + localCollection[i].Y) / 2;
            }

            return area;
        }

        private float ShapeArea()
        {
            return Math.Abs(SignedShapeArea());
        }
    }
}