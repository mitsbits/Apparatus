﻿using System;

namespace Borg.Framework.Storage.Assets
{
    public class VersionCreatedEventArgs<TKey> : EventArgs where TKey : IEquatable<TKey>
    {
        public VersionCreatedEventArgs(TKey recordId, int version)
        {
            RecordId = recordId;
            Version = version;
        }

        public TKey RecordId { get; }
        public int Version { get; }
    }
}