using Borg.Framework.Storage.Assets.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;

namespace Borg.Framework.Storage.Assets
{
    public class AssetInfoDefinition<TKey> : IAssetInfo<TKey> where TKey : IEquatable<TKey>
    {
        protected VersionInfoDefinition<TKey> _currentFile;

        public AssetInfoDefinition(TKey id, string name, DocumentBehaviourState state = DocumentBehaviourState.Commited)
        {
            Id = id;
            Name = Preconditions.NotEmpty(name, nameof(name));
            DocumentBehaviourState = state;
        }

        public string Name { get; set; }
        public DocumentBehaviourState DocumentBehaviourState { get; protected set; }

        public TKey Id { get; protected set; }

        IVersionInfo IAssetInfo.CurrentFile => CurrentFile;

        public virtual VersionInfoDefinition<TKey> CurrentFile
        {
            get => _currentFile;
            set => _currentFile = value;
        }

        public virtual CompositeKey Keys
        {
            get
            {
                var keys = new CompositeKey();
                keys.Add(nameof(Id), Id);
                return keys;
            }
        }

        IVersionInfo<TKey> IAssetInfo<TKey>.CurrentFile => CurrentFile;
    }
}