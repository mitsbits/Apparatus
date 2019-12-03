using Borg.Infrastructure.Core.Euclidean;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Infrastructure.Core.Tests.Euclidean
{
    public class PlanarTriangleTests : TestBase
    {
        public PlanarTriangleTests(ITestOutputHelper output) : base(output)
        {
        }
        [Theory]
        [InlineData(0, 0, 10, 0, 0, 10, 3.3333333333333335, 3.3333333333333335)]
        [InlineData(0, 0, -10, 0, 0, -10, -3.3333333333333335, -3.3333333333333335)]
        public void calculate_center(float x1, float y1, float x2, float y2, float x3, float y3, float resultX, float resultY)
        {
            var shape = new PlanarTriangle(new[] { new PlanarPoint(x1, y1), new PlanarPoint(x2, y2), new PlanarPoint(x3, y3) });
            var center = shape.Center();
            center.X.ShouldBe(resultX);
            center.Y.ShouldBe(resultY);
        }
    }
}
