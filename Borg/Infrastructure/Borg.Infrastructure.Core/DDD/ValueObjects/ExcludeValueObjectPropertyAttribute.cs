using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ExcludeValueObjectPropertyAttribute : ExcludeFromValueObjectAttribute
    {
    }
}