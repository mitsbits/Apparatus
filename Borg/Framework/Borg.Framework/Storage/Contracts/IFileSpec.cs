using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileSpec<out TKey> : IFileSpec, ICloneable<IFileSpec<TKey>> where TKey : IEquatable<TKey>
    {
        TKey Id { get; }
    }

    public interface IFileSpec : IHaveName, IHaveMimeType, IHaveExtension, IFileAccessTimeInfo
    {
        string FullPath { get; }

        long SizeInBytes { get; }
 
    }



    public interface IFileAccessTimeInfo
    {
        DateTimeOffset CreationDate { get; }
        DateTimeOffset LastWrite { get; }
        DateTimeOffset? LastRead { get; }
    }

    public interface IHaveMimeType
    {
        string MimeType { get; }
    }
    public interface IHaveExtension
    {
        string Extension { get; }
    }


}