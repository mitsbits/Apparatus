using Borg.Infrastructure.Core.Euclidean;
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
        public void calculate_angle(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float result)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var secant = new PlanarLine(new PlanarPoint(x3, y3), new PlanarPoint(x4, y4));
            var radius = line.CornerRadius(secant);
            radius.ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0, 0, 10, 0, 0, 0, 10, 0, 0)]
        public void calculate_intersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float resultX, float resultY)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var secant = new PlanarLine(new PlanarPoint(x3, y3), new PlanarPoint(x4, y4));
            var intersection = line.Intersection(secant);
            resultX.ShouldBe(intersection.X);
            resultY.ShouldBe(intersection.Y);
        }

        [Theory]
        [InlineData(0, 0, 0, 10, 10)]
        [InlineData(-10, 10, -10, -10, 20)]
        public void calculate_distance(float x1, float y1, float x2, float y2, float result)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var dist = line.GetDistance();
            dist.ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0, 0, 10, 0, 10)]
        [InlineData(-10, 10, -10, -10, 0, -20)]
        public void get_vector(float x1, float y1, float x2, float y2, float resultX, float resultY)
        {
            var line = new PlanarLine(new PlanarPoint(x1, y1), new PlanarPoint(x2, y2));
            var vector = line.Vector();
            vector.X.ShouldBe(resultX);
            vector.Y.ShouldBe(resultY);
        }
    }
}