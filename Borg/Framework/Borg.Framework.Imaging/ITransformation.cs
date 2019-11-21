using Borg.Framework.Storage.Contracts;
using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Imaging
{
    public interface ITransformation
    {
        Task Transform(Stream incoming, Stream outgoing);
    }

    public interface ImageInfo : IHaveName, IHaveMimeType, IHaveExtension, IHaveWidth, IHaveHeight
    {


    }

    public interface ICanGetStream
    {
        Task<Stream> GetStream();
    }
}
