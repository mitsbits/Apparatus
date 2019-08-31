using Borg.Framework.Storage.Contracts;

namespace Borg.Framework.Storage
{
    public class MimeTypeSpec : IMimeTypeSpec
    {
        public MimeTypeSpec(string extension, string mimeType)
        {
            Extension = extension;
            MimeType = mimeType;
        }

        public string Extension { get; }
        public string MimeType { get; }
    }
}