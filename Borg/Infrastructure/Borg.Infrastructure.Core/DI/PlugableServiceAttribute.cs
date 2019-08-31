using System;
using System.ComponentModel;

namespace Borg.Infrastructure.Core.DI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class PlugableServiceAttribute : Attribute
    {
        private Type _implementationOf;

        public PlugableServiceAttribute(Type implementationOf, Lifetime lifetime = Lifetime.Transient, bool oneOfMany = true, int order = 0) : this()
        {
            ImplementationOf = implementationOf;
            Lifetime = lifetime;
            OneOfMany = oneOfMany;
            Order = order;
        }

        public PlugableServiceAttribute()
        {
        }

        public Type ImplementationOf
        {
            get => _implementationOf; set
            {
                if (!value.IsInterface) throw new ArgumentException($"{nameof(value.FullName)} is not an Interface");
                _implementationOf = value;
            }
        }

        [DefaultValue(true)]
        public bool OneOfMany { get; set; } = true;

        [DefaultValue(Lifetime.Transient)]
        public Lifetime Lifetime { get; set; } = Lifetime.Transient;

        [DefaultValue(0)]
        public int Order { get; set; } = 0;
    }
}