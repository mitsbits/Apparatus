using Borg;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Test.Borg.Infrastructure.Core
{
    public class NumericExtensionsTests : TestBase
    {
        public NumericExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1, "1st")]
        [InlineData(2, "2nd")]
        [InlineData(3, "3rd")]
        [InlineData(4, "4th")]
        [InlineData(5, "5th")]
        [InlineData(10, "10th")]
        [InlineData(11, "11th")]
        [InlineData(12, "12th")]
        [InlineData(13, "13th")]
        public void ToOrdinalTest(int source, string output)
        {
            source.ToOrdinal().ShouldBe(output);
        }

        [Theory]
        [InlineData(1, "1 B")]
        [InlineData(1 * 1024, "1 KB")]
        [InlineData(1 * 1024 * 1024, "1 MB")]
        [InlineData(1 * 1024 * 1024 * 1024, "1 GB")]
        public void SizeDisplayTests(long source, string output)
        {
            source.SizeDisplay().ShouldBe(output);
        }

        [Theory]
        [InlineData(1, 10, 10)]
        [InlineData(9, 10, 10)]
        [InlineData(10, 10, 20)]
        [InlineData(1, 30, 30)]
        [InlineData(28, 30, 30)]
        [InlineData(30, 30, 60)]
        [InlineData(99, 100, 100)]
        [InlineData(101, 100, 200)]
        public void RoundUp(long source, int round, long output)
        {
            source.RoundUp(round).ShouldBe(output);
        }

        [Theory]
        [InlineData(1, 10, 0)]
        [InlineData(9, 10, 10)]
        [InlineData(10, 10, 10)]
        [InlineData(1, 30, 0)]
        [InlineData(28, 30, 30)]
        [InlineData(30, 30, 30)]
        [InlineData(99, 100, 100)]
        [InlineData(101, 100, 100)]
        public void RoundOff(long source, int round, long output)
        {
            source.RoundOff(round).ShouldBe(output);
        }

        [Theory]
        [InlineData(1, 10, 0)]
        [InlineData(9, 10, 0)]
        [InlineData(10, 10, 10)]
        [InlineData(1, 30, 0)]
        [InlineData(28, 30, 0)]
        [InlineData(30, 30, 30)]
        [InlineData(99, 100, 0)]
        [InlineData(101, 100, 100)]
        public void RoundDown(long source, int round, long output)
        {
            source.RoundDown(round).ShouldBe(output);
        }
    }
}