using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExcludeValueObjectFieldAttribute : Attribute
    {
    }
}
