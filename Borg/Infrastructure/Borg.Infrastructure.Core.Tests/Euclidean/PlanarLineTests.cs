using Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean;
using Shouldly;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Infrastructure.Core.Tests.Euclidean
{
    public class PlanarLineTests : TestBase
    {
        public PlanarLineTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(0, 0, 10, 0, 0, 10, 3.3333333333333335, 3.3333333333333335)]
        [InlineData(0, 0, -10, 0, 0, -10, -3.3333333333333335, -3.3333333333333335)]
        public void calculate_center(double x1, double y1, double x2, double y2, double x3, double y3, double resultX, double resultY)
        {
            var shape = new PlanarTriangle(new[] { new PlanarPoint(x1, y1), new PlanarPoint(x2, y2), new PlanarPoint(x3, y3) });
            var center = shape.Center();
            center.X.ShouldBe(resultX);
            center.X.ShouldBe(resultX);
        }
    }
}