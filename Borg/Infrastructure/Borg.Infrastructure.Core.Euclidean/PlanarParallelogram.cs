using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infrastructure.Core.Euclidean
{
    public class PlanarParallelogram : PlanarShape
    {
        public PlanarParallelogram(IEnumerable<PlanarPoint> points) : base(points)
        {

        }

        protected override PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() != 4) throw new InvalidOperationException($"Three distinct points are required for a {nameof(PlanarΤrapezoid)}");
            return distinctPoints.ToArray();
        }
    }

    public class PlanarSquare : PlanarParallelogram
    {
        public PlanarSquare(IEnumerable<PlanarPoint> points) : base(points)
        {

        }
    }
}
