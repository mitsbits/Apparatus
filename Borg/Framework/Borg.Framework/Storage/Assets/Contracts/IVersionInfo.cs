using Borg.Framework.Storage.Contracts;
using System;

namespace Borg.Framework.Storage.Assets.Contracts
{
    public interface IVersionInfo
    {
        int Version { get; }

        IFileSpec FileSpec { get; }
    }

    public interface IVersionInfo<out TKey> : IVersionInfo where TKey : IEquatable<TKey>
    {
        new IFileSpec<TKey> FileSpec { get; }
    }
}