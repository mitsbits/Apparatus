using Borg.Infrastructure.Core.DDD.Contracts;
using System;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileSpec<out TKey> : IFileSpec, ICloneable<IFileSpec<TKey>> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IFileSpec
    {
        string FullPath { get; }
        string Name { get; }
        DateTimeOffset CreationDate { get; }
        DateTimeOffset LastWrite { get; }
        DateTimeOffset? LastRead { get; }
        long SizeInBytes { get; }
        string MimeType { get; }
        string Extension { get; }
    }
}