using Borg;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Test.Borg.Infrastructure.Core.Reflection
{
    public class GetGenericArgumentTypeTest : TestBase
    {
        public GetGenericArgumentTypeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void check_that_method_returns_null_for_non_generic()
        {
            var result = typeof(One).GetGenericArgumentType(0);
            result.ShouldBeNull();
        }

        [Fact]
        public void check_that_method_returns_first_property_type_on_generic()
        {
            var result = typeof(Three<One>).GetGenericArgumentType(0);
            result.ShouldBe(typeof(One));
        }

        [Fact]
        public void check_that_method_returns_first_property_type_on_generic_collection()
        {
            var result = typeof(IList<Two>).GetGenericArgumentType(0);
            result.ShouldBe(typeof(Two));
        }

        [Fact]
        public void check_that_method_returns_second_property_type_on_generic_collection()
        {
            var result = typeof(Four<One, Two>).GetGenericArgumentType(1);
            result.ShouldBe(typeof(Two));
        }

        [Fact]
        public void check_that_method_returns_throws_out_of_index_exception()
        {
            Should.Throw<IndexOutOfRangeException>(() =>
            {
                var result = typeof(Four<One, Two>).GetGenericArgumentType(2);
            });
        }

        private class One
        { }

        private class Two : One
        { }

        private class Three<T> where T : One
        { }

        private class Four<T, W> : Three<T> where T : One where W : Two
        { }
    }
}