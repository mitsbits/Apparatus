using System;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ExcludeValueObjectFieldAttribute : ExcludeFromValueObjectAttribute
    {
    }
}