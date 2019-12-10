using Borg.Infrastructure.Core;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace Borg.Framework.EF.Instructions.Attributes.Schema
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PrincipalForeignKeyDefinitionAttribute : Attribute
    {
        public PrincipalForeignKeyDefinitionAttribute(bool IsRequired, params string[] columns)
        {
            Columns = Preconditions.NotEmpty(columns, nameof(columns)).ToArray();
        }

        public string[] Columns { get; }
        public bool IsRequired { get; set; } = false;
    }
}