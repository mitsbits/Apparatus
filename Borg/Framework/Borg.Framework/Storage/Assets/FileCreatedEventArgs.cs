using System;

namespace Borg.Framework.Storage.Assets
{
    public class FileCreatedEventArgs<TKey> : EventArgs where TKey : IEquatable<TKey>
    {
        public FileCreatedEventArgs(TKey recordId, string mimeType)
        {
            RecordId = recordId;
            MimeType = mimeType;
        }

        public TKey RecordId { get; }
        public string MimeType { get; }
    }
}