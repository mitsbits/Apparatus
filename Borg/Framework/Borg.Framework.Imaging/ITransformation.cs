using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.Imaging
{
    public interface ITransformation
    {
    }

    public interface ImageInfo : IHaveName
    {
        string MimeType { get; }
        string Extension { get; }
    }
}
