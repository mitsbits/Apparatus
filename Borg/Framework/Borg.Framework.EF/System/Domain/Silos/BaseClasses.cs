using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;

using System;

namespace Borg.Framework.EF.System.Domain.Silos
{
    public abstract class Siloed : IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public virtual int LanguageId { get; protected set; }
        public virtual int TenantId { get; protected set; }

        public virtual Language? Language { get; protected set; }
        public virtual Tenant? Tenant { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithId(Id)
            .AddKey(nameof(LanguageId)).AddValue(LanguageId)
            .AddKey(nameof(TenantId)).AddValue(TenantId)
            .Build();
    }

    public abstract class SiloedActivatable : Siloed, IActivatable
    {
        public DateTimeOffset? ActiveFrom { get; protected set; }

        public DateTimeOffset? ActiveTo { get; protected set; }

        public string ActivationID { get; protected set; } = string.Empty;

        public string DeActivationID { get; protected set; } = string.Empty;

        public bool IsActive { get; protected set; }
        public bool IsCurrentlyActive { get; protected set; }
    }

    public abstract class Treenode : Siloed, ITreeNode
    {
        public int? ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }

    public abstract class TreenodeActivatable : SiloedActivatable, ITreeNode
    {
        public int? ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }

    public abstract class SlugTreenodeActivatable : TreenodeActivatable, IHaveSlug, IHaveFullSlug
    {
        public string Slug { get; protected set; }

        public string FullSlug { get; protected set; }
    }
}