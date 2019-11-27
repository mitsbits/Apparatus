using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Infrastructure.Core.Indexing
{
    public interface IIndexable
    {
    }

    public interface IFieldValueProvider<in IIdentifiable, out IIndexable>
    {
        string Field { get; }
        object FieldValue(IIdentifiable entity);

    }

    public class SimpleTypes
    {
        public const string Byte = "byte";
        public const string Sbyte = "sbyte";
        public const string Short = "short";
        public const string Ushort = "ushort";
        public const string Int = "int";
        public const string Uint = "uint";
        public const string Long = "long";
        public const string Ulong = "ulong";
        public const string Float = "float";
        public const string Double = "double";
        public const string Decimal = "decimal";
        public const string Char = "char";
        public const string Bool = "bool";
        public const string String = "string";
        public const string DateTime = "DateTime";
    }
}
