using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
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

        public override PlanarPoint Center()
        {
            return TriangleCenter(CenterType.Centroid);
        }

        private PlanarPoint TriangleCenter(CenterType centerType = CenterType.Centroid)
        {
            PlanarPoint result;
            switch (centerType)
            {
                case CenterType.Centroid:
                    var x = (Points[0].X + Points[1].X + Points[2].X) / 3;
                    var y = (Points[0].Y + Points[1].Y + Points[2].Y) / 3;
                    return new PlanarPoint(x, y);
                    break;

                default:
                    throw new NotImplementedException($"Can not calculate {nameof(TriangleCenter)} for {centerType.ToString()}");
                    break;
            }
        }

        public enum CenterType
        {
            Centroid = 0, //default
            Incenter = 1,
            Circumcenter = 2,
            Orthocenter = 3,
            EulerLine = 4,
            CevasTheorem = 5,
            Addendum = 6
        }
    }
}