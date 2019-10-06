using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean
{
    public class PlanarLine : ValueObject<PlanarPoint>
    {
        public PlanarLine(PlanarPoint pointOne, PlanarPoint pointTwo)
        {
            if (pointOne.Equals(pointTwo))
            {
                throw new InvalidOperationException($"this is not a line"); //TODO: create suitable exception
            }
            PointOne = Preconditions.NotNull(pointOne, nameof(pointOne));
            PointTwo = Preconditions.NotNull(pointTwo, nameof(pointTwo));
        }

        public PlanarPoint PointOne { get; }
        public PlanarPoint PointTwo { get; }
    }
}