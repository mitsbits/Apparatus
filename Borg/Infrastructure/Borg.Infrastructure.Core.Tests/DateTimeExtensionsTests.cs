using Borg;
using Shouldly;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Test.Borg.Infrastructure.Core
{
    public class DateTimeExtensionsTests : TestBase
    {
        public DateTimeExtensionsTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RoundUp()
        {
            new DateTime(2019, 1, 1, 0, 0, 0).RoundUp(TimeSpan.FromMinutes(30)).ShouldBe(new DateTime(2019, 1, 1, 0, 30, 0));
        }
    }
}