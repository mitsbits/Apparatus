using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Silos;
using System;

namespace Borg.Platform.EF
{
    public abstract class Siloed : IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual int LanguageId { get; protected set; }
        public virtual int TenantId { get; protected set; }

        public virtual Language Language { get; protected set; }
        public virtual Tenant Tenant { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithFieldName(nameof(Id))
            .AddValue(Id)
            .AddKey(nameof(LanguageId)).AddValue(LanguageId)
            .AddKey(nameof(TenantId)).AddValue(TenantId)
            .Build();
    }

    public abstract class SiloedActivatable : Siloed, IActivatable
    {
        public DateTimeOffset? ActiveFrom { get; protected set; }

        public DateTimeOffset? ActiveTo { get; protected set; }

        public string ActivationID { get; protected set; }

        public string DeActivationID { get; protected set; }

        public bool IsActive { get; protected set; }
    }

    public abstract class Treenode : Siloed, ITreeNode<int>
    {
        public int ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }

    public abstract class TreenodeActivatable : SiloedActivatable, ITreeNode<int>
    {
        public int ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }
}