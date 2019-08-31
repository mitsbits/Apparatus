using Borg.Framework.Storage.Assets.Contracts;
using Borg.Framework.Storage.Contracts;
using System;

namespace Borg.Framework.Storage.Assets
{
    public class VersionInfoDefinition : IVersionInfo
    {
        public VersionInfoDefinition(int version, IFileSpec fileSpec)
        {
            Version = version;
            FileSpec = fileSpec;
        }

        public int Version { get; }
        public IFileSpec FileSpec { get; }
    }

    public class VersionInfoDefinition<TKey> : VersionInfoDefinition, IVersionInfo<TKey> where TKey : IEquatable<TKey>
    {
        public VersionInfoDefinition(int version, IFileSpec<TKey> fileSpec) : base(version, fileSpec)
        {
            FileSpec = fileSpec;
        }

        public new IFileSpec<TKey> FileSpec { get; }
    }
}