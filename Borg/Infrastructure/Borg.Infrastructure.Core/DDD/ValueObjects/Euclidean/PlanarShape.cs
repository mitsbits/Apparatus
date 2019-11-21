using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Borg.Infrastructure.Core.DDD.Enums;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public abstract class PlanarShape : ValueObject<PlanarShape>
    {
        protected PlanarShape(IEnumerable<PlanarPoint> points)
        {
            Points = ValidatePoints(Preconditions.NotEmpty(points, nameof(points)));
        }
        public PlanarPoint[] Points { get; private set; }

        protected virtual PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() < 2) throw new InvalidOperationException($"At least two points are required for a {nameof(PlanarShape)}");
            return distinctPoints.ToArray();
        }

        private PlanarPoint Center()
        {
            var x = Righter().X - Lefter().X;
            var y = Higher().Y - Lower().Y;
            return new PlanarPoint(x, y);
        }

        private PlanarShape CenterToLefter()
        {
            var point = Lefter();
            var zero = PlanarPoint.Zero();
            var horizontalOperation = zero.X - point.X;
            var verticalOperation = zero.Y - point.Y;
            var newCollection = Points.Select(x => new PlanarPoint(x.X + horizontalOperation, x.Y + verticalOperation));
            if (newCollection.Count() == 2) return new PlanarLine(newCollection.First(), newCollection.Skip(1).First());
            if (newCollection.Count() == 3) return new PlanarTriangle(newCollection);
            return new PlanarRectangular(newCollection);
        }

        private IEnumerable<PlanarPoint> Quadrants()
        {
            if (GetType().Equals(typeof(PlanarLine)))
            {
                return new[] { Lefter(), Righter() };
            }
            if (GetType().Equals(typeof(PlanarTriangle)))
            {
                List< PlanarPoint> quadrant1;
                List<PlanarPoint> quadrant2;
                List<PlanarPoint> quadrant3;
                List<PlanarPoint> quadrant4;
                PlanarPoint point1;
                PlanarPoint point2;
                PlanarPoint point3;
                point1 = Lefter();
                var remaining = Points.Where(x => x != point1);
                point2 = remaining.OrderByDescending(x => x.X).First();
                point3 = remaining.OrderBy(x => x.X).First();




            }
        }

        private decimal FurtherPointsVlaue(Compass orientation)
        {
            decimal result;
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
            return eligibles.OrderBy(x=>x.X).First();
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
    }

    public sealed class PlanarTriangle : PlanarShape
    {
        public PlanarTriangle(IEnumerable<PlanarPoint> points) : base(points)
        {

        }

        protected override PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() != 3) throw new InvalidOperationException($"Three distinct points are required for a {nameof(PlanarTriangle)}");
            return distinctPoints.ToArray();
        }
    }

    public sealed class PlanarRectangular : PlanarShape
    {
        public PlanarRectangular(IEnumerable<PlanarPoint> points) : base(points)
        {

        }

        protected override PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() != 4) throw new InvalidOperationException($"Three distinct points are required for a {nameof(PlanarRectangular)}");
            return distinctPoints.ToArray();
        }
    }
}
