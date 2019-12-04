using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infrastructure.Core.Euclidean
{
    public sealed class PlanarΤrapezoid : PlanarShape //parallelogram
    {
        public PlanarΤrapezoid(IEnumerable<PlanarPoint> points) : base(points)
        {
        }

        protected override PlanarPoint[] ValidatePoints(IEnumerable<PlanarPoint> points)
        {
            var distinctPoints = points.Distinct();
            if (distinctPoints.Count() != 4) throw new InvalidOperationException($"Three distinct points are required for a {nameof(PlanarΤrapezoid)}");
            return distinctPoints.ToArray();
        }
    }
}