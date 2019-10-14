using Borg.Framework.Storage.FileDepots;
using Borg.Framework.Storage.FileProviders;
using Shouldly;
using System;
using System.Linq;
using System.Reflection;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Framework.Tests.Storage
{
    public class MemoryDepotTests : TestBase
    {
        public MemoryDepotTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        private void test_that_a_new_provider_contains_root_directory()
        {
            using (var depot = new MemoryFileDepot())
            {
                Should.NotThrow(() =>
               {
                   var contents = depot.GetDirectoryContents(string.Empty);
                   contents.Exists.ShouldBeTrue();
                   contents.Count().ShouldBe(1);
                   contents.Single().IsDirectory.ShouldBeTrue();
                   contents.Single().Name.ShouldBe(string.Empty);
                   contents.Single().PhysicalPath.ShouldBe("/");
               });
            }
        }

        [Fact]
        private void test_creating_a_directory_at_the_root()
        {
            using (var depot = new MemoryFileDepot())
            {
                Should.NotThrow(async () =>
                {
                    var info = await depot.Save("subdirectory", null);
                    info.IsDirectory.ShouldBeTrue();
                    info.Exists.ShouldBeTrue();
                    info.Name.ShouldBe("subdirectory");
                    info.PhysicalPath.ShouldBe("/subdirectory/");
                    var directory = depot.GetDirectoryContents("subdirectory");
                    directory.Exists.ShouldBeTrue();
                });
            }
        }

        [Fact]
        private void test_saving_a_file_at_the_root()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Borg.Framework.Tests.Storage.Files.n.jpg";

            using (var depot = new MemoryFileDepot())
            {
                Should.NotThrow(async () =>
               {
                   using (var stream = assembly.GetManifestResourceStream(resourceName))
                   {
                       var path = "n.jpg";
                       var info = await depot.Save(path, stream);
                       info.Name.ShouldBe("n");
                       info.Exists.ShouldBeTrue();
                       info.IsDirectory.ShouldBeFalse();
                       info.Length.ShouldBeGreaterThan(0);

                       var fileInfo = info as MemoryFileInfo;
                       fileInfo.ShouldNotBeNull();
                       fileInfo.Exists.ShouldBeTrue();
                       fileInfo.PhysicalPath.ShouldBe("/n.jpg");
                       fileInfo.Name.ShouldBe("n");
                       fileInfo.Extension.ShouldBe(".jpg");
                       fileInfo.Length.ShouldBeGreaterThan(0);

                       var directoyContents = depot.GetDirectoryContents(string.Empty);
                       var file = directoyContents.FirstOrDefault(x => x.PhysicalPath == "/n.jpg") as MemoryFileInfo;
                       file.ShouldNotBeNull();
                       file.Exists.ShouldBeTrue();
                       file.PhysicalPath.ShouldBe("/n.jpg");
                       file.Name.ShouldBe("n");
                       file.Extension.ShouldBe(".jpg");
                       file.Length.ShouldBeGreaterThan(0);
                   }
               });
            }
        }

        [Fact]
        private void test_saving_a_file_at_a_subpath()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Borg.Framework.Tests.Storage.Files.n.jpg";

            using (var depot = new MemoryFileDepot())
            {
                Should.NotThrow(async () =>
                {
                    using (var stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        var path = "apath/asubpath/anothersubpath/n.jpg";
                        var info = await depot.Save(path, stream);
                        info.Name.ShouldBe("n");
                        info.Exists.ShouldBeTrue();
                        info.IsDirectory.ShouldBeFalse();
                        info.Length.ShouldBeGreaterThan(0);

                        var fileInfo = info as MemoryFileInfo;
                        fileInfo.ShouldNotBeNull();
                        fileInfo.Extension.ShouldBe(".jpg");

                        var first = depot.GetDirectoryContents("apath");
                        first.Exists.ShouldBeTrue();
                        var directory = first.First();
                        directory.Name.ShouldBe("apath");
                        directory.PhysicalPath.ShouldBe("/apath/");
                        directory = first.Skip(1).Take(1).Single();
                        directory.Name.ShouldBe("asubpath");
                        directory.PhysicalPath.ShouldBe("/apath/asubpath/");

                        var directoyContents = depot.GetDirectoryContents("apath/asubpath/anothersubpath/");
                        var fileinfo = directoyContents.Skip(1).Take(1).Single() as MemoryFileInfo;
                        fileInfo.ShouldNotBeNull();
                        fileInfo.Exists.ShouldBeTrue();
                        fileInfo.PhysicalPath.ShouldBe("/apath/asubpath/anothersubpath/n.jpg");
                        fileInfo.Name.ShouldBe("n");
                        fileInfo.Extension.ShouldBe(".jpg");
                        fileInfo.Length.ShouldBeGreaterThan(0);
                    }
                });
            }
        }

        [Fact]
        private void test_that_an_invalid_path_returns_not_found_result()
        {
            using (var depot = new MemoryFileDepot())
            {
                var info = depot.GetFileInfo("an/imvalid.path");
                info.Exists.ShouldBeFalse();
                Should.Throw<InvalidOperationException>(() =>
              {
                  var stream = info.CreateReadStream();
              });
            }
        }
    }
}