using Borg.Infrastructure.Core.DDD.ValueObjects.Euclidean;
using Shouldly;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Infrastructure.Core.Tests.Euclidean
{
    public class PlanarPointTests : TestBase
    {
        public PlanarPointTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(0, 0, 10, 10, 10, 10)]
        [InlineData(0, 0, -10, -10, -10, -10)]
        [InlineData(10, 10, -10, -10, 0, 0)]
        [InlineData(10, -10, 10, -10, 20, -20)]
        public void add_points(double x1, double y1, double x2, double y2, double resultX, double resultY)
        {
            var point1 = new PlanarPoint(x1, y1);
            var point2 = new PlanarPoint(x2, y2);
            var result = point1 + point2;
            result.X.ShouldBe(resultX);
            result.Y.ShouldBe(resultY);
        }

        [Theory]
        [InlineData(0, 0, 10, 10, -10, -10)]
        [InlineData(0, 0, -10, -10, 10, 10)]
        [InlineData(10, 10, -10, -10, 20, 20)]
        [InlineData(10, -10, 10, -10, 0, 0)]
        [InlineData(10, -10, -10, 10, 20, -20)]
        public void substract_points(double x1, double y1, double x2, double y2, double resultX, double resultY)
        {
            var point1 = new PlanarPoint(x1, y1);
            var point2 = new PlanarPoint(x2, y2);
            var result = point1 - point2;
            result.X.ShouldBe(resultX);
            result.Y.ShouldBe(resultY);
        }
    }
}