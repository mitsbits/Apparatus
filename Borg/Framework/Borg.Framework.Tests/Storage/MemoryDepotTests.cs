using Borg.Framework.Storage.Contracts;
using Borg.Framework.Storage.FileDepots;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Framework.Tests.Storage
{
    public class MemoryDepotTests : TestBase
    {
        public MemoryDepotTests(ITestOutputHelper output) : base(output)
        {
            var serviceProviderBuilder = new ServiceCollection().AddSingleton<IFileDepot, MemoryFileDepot>();
            Provider = serviceProviderBuilder.BuildServiceProvider();
        }

        private IServiceProvider Provider { get; set; }

        [Fact]
        private void test_that_a_new_provider_contains_root_directory()
        {
            using (var depot = new MemoryFileDepot())
            {
                var contents = depot.GetDirectoryContents(string.Empty);
                contents.Exists.ShouldBeTrue();
                contents.Count().ShouldBe(1);
                contents.Single().IsDirectory.ShouldBeTrue();
                contents.Single().Name.ShouldBe(string.Empty);
                contents.Single().PhysicalPath.ShouldBe("/");
            }
        }

        [Fact]
        private void test_creating_a_directory_at_the_root()
        {
            using (var depot = new MemoryFileDepot())
            {
                Should.NotThrow(async () =>
                {
                    var saveresult = await depot.Save("subdirectory", null);
                    saveresult.ShouldBeTrue();
                    var directory = depot.GetDirectoryContents("subdirectory");
                    directory.Exists.ShouldBeTrue();
                });
            }
        }
    }
}