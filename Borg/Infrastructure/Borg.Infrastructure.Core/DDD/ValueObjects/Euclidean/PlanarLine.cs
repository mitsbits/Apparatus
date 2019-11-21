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
    }
}