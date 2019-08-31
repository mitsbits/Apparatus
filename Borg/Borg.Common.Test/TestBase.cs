using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Xunit;
using System;
using System.IO;
using System.Reflection;
using Xunit.Abstractions;

namespace Test.Borg
{
    public abstract class TestBase
    {
        protected readonly ILoggerFactory _moqLoggerFactory;

        private readonly Lazy<string> _absoluteBin = new Lazy<string>(() =>
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath));
            return directory.FullName;
        });

        public TestBase(ITestOutputHelper output)
        {
            _moqLoggerFactory = new LoggerFactory();
            _moqLoggerFactory.AddProvider(new XunitLoggerProvider(output));
        }

        protected string AbsoluteBin => _absoluteBin.Value;
    }
}