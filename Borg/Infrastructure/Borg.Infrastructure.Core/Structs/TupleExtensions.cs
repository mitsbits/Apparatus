using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg
{
    public static class TupleExtensions
    {
        private static readonly HashSet<Type> ValueTupleTypes = new HashSet<Type>(new Type[]
        {
        typeof(ValueTuple<>),
        typeof(ValueTuple<,>),
        typeof(ValueTuple<,,>),
        typeof(ValueTuple<,,,>),
        typeof(ValueTuple<,,,,>),
        typeof(ValueTuple<,,,,,>),
        typeof(ValueTuple<,,,,,,>),
        typeof(ValueTuple<,,,,,,,>)
        });

        public static bool IsValueTuple(this object obj) => IsValueTupleType(obj.GetType());

        public static bool IsValueTupleType(this TypeInfo type)
        {
            return type.IsGenericType && ValueTupleTypes.Contains(type.GetGenericTypeDefinition());
        }

        public static bool IsValueTupleType(this Type type)
        {
            return type.GetTypeInfo().IsValueTupleType();
        }

        public static bool IsValueTupleType(this PropertyInfo type)
        {
            return type.PropertyType.IsValueTupleType();
        }

        //public static List<object> GetValueTupleItemObjects(this object tuple) => GetValueTupleItemFields(tuple.GetType()).Select(f => f.GetValue(tuple)).ToList();

        //public static List<Type> GetValueTupleItemTypes(this Type tupleType) => GetValueTupleItemFields(tupleType).Select(f => f.FieldType).ToList();

        public static List<FieldInfo> GetValueTupleItemFields(this Type tupleType)
        {
            var items = new List<FieldInfo>();

            FieldInfo field;
            int nth = 1;
            while ((field = tupleType.GetRuntimeField($"Item{nth}")) != null)
            {
                nth++;
                items.Add(field);
            }

            return items;
        }
    }
}