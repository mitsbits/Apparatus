using Borg;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Test.Borg.Infrastructure.Core
{
    public class StringExtensionsTests : TestBase
    {
        public StringExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RemoveWhitespace()
        {
            var source = "ab c d ef gh i ";
            var target = "abcdefghi";
            source.RemoveWhitespace().ShouldBe(target);
        }

        [Fact]
        public void EqualsWhitespaceAgnostic()
        {
            var source = "ab c d ef gh i ";
            var target = "abcdefghi";
            source.EqualsWhitespaceAgnostic(target).ShouldBeTrue();
        }

        [Fact]
        public void Repeat()
        {
            var source = "a";
            var target = "aaaaa";
            source.Repeat(5).ShouldBe(target);
        }

        [Fact]
        public void IsNullOrWhiteSpace()
        {
            string.Empty.IsNullOrWhiteSpace().ShouldBeTrue();
            "aaaaa".IsNullOrWhiteSpace().ShouldBeFalse();
            "   ".IsNullOrWhiteSpace().ShouldBeTrue();
        }

        [Fact]
        public void IndexOfWhitespaceAgnostic()
        {
            var source = new[] { " a", "b ", "c ", " d", "e ", " f" };
            source.IndexOfWhitespaceAgnostic("e").ShouldBe(4);
            source.IndexOfWhitespaceAgnostic("g").ShouldBe(-1);
        }

        [Fact]
        public void ContainsWhitespaceAgnostic()
        {
            var source = new[] { " a", "b ", "c ", " d", "e ", " f" };
            source.ContainsWhitespaceAgnostic("e").ShouldBeTrue();
            source.ContainsWhitespaceAgnostic("g").ShouldBeFalse();
        }

        //[Fact]
        //public void DistinctWhitespaceAgnostic()
        //{
        //    var source = new[] { " a", " a ", "b ", "   b ", "c ", "     c ", " d", "e " };
        //    source.DistinctWhitespaceAgnostic().ShouldBe(new[] { "a", "b", "c", "d", "e" });
        //}
    }
}