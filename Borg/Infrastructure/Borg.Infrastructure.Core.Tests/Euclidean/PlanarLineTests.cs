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
        [InlineData(0, 0, 0, 10, 0, 0, 10, 0, 90)]
        //[InlineData(-10, -10, 10, 10)]
        //[InlineData(10, -10, -10, 10)]
        public void calculate_angle(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, double result)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var secant = new PlanarLine(new PlanarPoint(x3, y3), new PlanarPoint(x4, y4));
            var radius = line.CornerRadius(secant);
            radius.ShouldBe(result);
        }
        [Theory]
        [InlineData(0, 0, 0, 10, 0, 0, 0, 10, 0, 0)]
        public void calculate_intersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, double resultX, double resultY)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var secant = new PlanarLine(new PlanarPoint(x3, y3), new PlanarPoint(x4, y4));
            var intersection = line.Intersection(secant);
            resultX.ShouldBe(intersection.X);
            resultY.ShouldBe(intersection.Y);
        }
    }
}