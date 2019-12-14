using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Borg
{
    public static class ReflectionExtensions
    {
        public static bool IsNonAbstractClass(this Type type, bool publicOnly = false)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsClass && !typeInfo.IsAbstract)
            {
                if (typeInfo.IsGenericType && typeInfo.ContainsGenericParameters)
                {
                    return false;
                }

                if (publicOnly)
                {
                    return typeInfo.IsPublic || typeInfo.IsNestedPublic;
                }

                return true;
            }

            return false;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
            {
                yield return implementedInterface;
            }

            var baseType = typeInfo.BaseType;

            while (baseType != null)
            {
                var baseTypeInfo = baseType.GetTypeInfo();

                yield return baseType;

                baseType = baseTypeInfo.BaseType;
            }
        }

        public static bool IsInNamespace(this Type type, string @namespace)
        {
            var typeNamespace = type.Namespace ?? string.Empty;

            if (@namespace.Length > typeNamespace.Length)
            {
                return false;
            }

            var typeSubNamespace = typeNamespace.Substring(0, @namespace.Length);

            if (typeSubNamespace.Equals(@namespace, StringComparison.Ordinal))
            {
                if (typeNamespace.Length == @namespace.Length)
                {
                    //exactly the same
                    return true;
                }

                //is a subnamespace?
                return typeNamespace[@namespace.Length] == '.';
            }

            return false;
        }

        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.GetTypeInfo().IsDefined(attributeType, inherit: true);
        }

        public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes<T>(inherit: true).Any(predicate);
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes<T>(inherit: true).Any();
        }

        public static bool HasAttribute<T>(this FieldInfo field) where T : Attribute
        {
            return field.GetCustomAttributes<T>(inherit: true).Any();
        }

        public static bool IsAssignableTo(this Type type, Type otherType)
        {
            var typeInfo = type.GetTypeInfo();
            var otherTypeInfo = otherType.GetTypeInfo();

            if (otherTypeInfo.IsGenericTypeDefinition)
            {
                if (typeInfo.IsGenericTypeDefinition)
                {
                    return typeInfo.Equals(otherTypeInfo);
                }

                return typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo);
            }

            return otherTypeInfo.IsAssignableFrom(typeInfo);
        }

        public static IEnumerable<Type> FindMatchingInterface(this TypeInfo typeInfo, Action<TypeInfo, IImplementationTypeFilter> action)
        {
            var matchingInterfaceName = $"I{typeInfo.Name}";

            var matchedInterfaces = GetImplementedInterfacesToMap(typeInfo)
                .Where(x => string.Equals(x.Name, matchingInterfaceName, StringComparison.Ordinal))
                .ToArray();

            Type type;
            if (action != null)
            {
                var filter = new ImplementationTypeFilter(matchedInterfaces);

                action(typeInfo, filter);

                type = filter.Types.FirstOrDefault();
            }
            else
            {
                type = matchedInterfaces.FirstOrDefault();
            }

            if (type != null)
            {
                yield return type;
            }
        }

        public static Type GetGenericArgumentType(this PropertyInfo type, int index = 0)
        {
            return type.PropertyType.GetGenericArgumentType(index);
        }

        public static Type GetGenericArgumentType(this Type type, int index = 0)
        {
            if (!type.IsOpenGeneric()) return null;
            index = Preconditions.PositiveOrZero(index, nameof(index));
            var args = type.GetTypeInfo().GenericTypeArguments;
            if (index >= args.Length)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            return args[index];
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsSubclassOfRawGeneric(this Type thisType, Type genericTypeToCheck)
        {
            while (thisType != null && thisType != typeof(object))
            {
                var cur = thisType.IsGenericType ? thisType.GetGenericTypeDefinition() : thisType;
                if (cur.IsAssignableFrom(genericTypeToCheck))
                {
                    return true;
                }
                thisType = thisType.BaseType;
            }
            return false;
        }

        public static bool ImplementsInterface(this Type thisType, Type interfaceTypeToCheck)
        {
            while (thisType != null && thisType != typeof(object))
            {
                Type cur = thisType.IsGenericType ? thisType.GetGenericTypeDefinition() : thisType;

                var interfaces = cur.GetInterfaces().ToList();

                if (interfaces.Contains(interfaceTypeToCheck))
                    return true;

                thisType = thisType.BaseType;
            }
            return false;
        }

        public static bool ImplementsInterface<T>(this Type thisType)
        {
            return thisType.ImplementsInterface(typeof(T));
        }

        public static bool IsDerivedFrom(this Type thisType, Type genericTypeToCheck)
        {
            Type typeToCheck = genericTypeToCheck.IsGenericType ? genericTypeToCheck.GetGenericTypeDefinition() : genericTypeToCheck;

            while (thisType != null && thisType != typeof(object))
            {
                Type cur = thisType.IsGenericType ? thisType.GetGenericTypeDefinition() : thisType;

                if (cur == typeToCheck)
                {
                    return true;
                }
                thisType = thisType.BaseType;
            }
            return false;
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool IsSimple(this PropertyInfo type)
        {
            return type.PropertyType.IsSimple();
        }

        public static bool IsSimple(this Type type)
        {
            return type.GetTypeInfo().IsSimple();
        }

        public static bool IsSimple(this TypeInfo type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple((type.GetGenericArguments()[0]).GetTypeInfo());
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || type.Equals(typeof(Guid));
        }

        public static bool IsEnumerator(this PropertyInfo type)
        {
            return type.PropertyType.IsEnumerator();
        }

        public static bool IsEnumerator(this Type type)
        {
            return type.GetTypeInfo().IsEnumerator();
        }

        public static bool IsEnumerator(this TypeInfo type)
        {
            if (type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))) return false;

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static string DisplayName(this Type type)
        {
            return TypeHelper.GetTypeDisplayName(type);
        }

        public static PropertyInfo GetPropertyInfo<T>(this T obj, Expression<Func<T, object>> selector)
        {
            if (selector.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Selector must be lambda expression", "selector");
            }

            var lambda = (LambdaExpression)selector;

            var memberExpression = ExtractMemberExpression(lambda.Body);

            if (memberExpression == null)
            {
                throw new ArgumentException("Selector must be member access expression", "selector");
            }

            if (memberExpression.Member.DeclaringType == null)
            {
                throw new InvalidOperationException("Property does not have declaring type");
            }

            return memberExpression.Member.DeclaringType.GetProperty(memberExpression.Member.Name);
        }

        #region Private

        private static bool IsAssignableToGenericTypeDefinition(this TypeInfo typeInfo, TypeInfo genericTypeInfo)
        {
            var interfaceTypes = typeInfo.ImplementedInterfaces.Select(t => t.GetTypeInfo());

            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType)
                {
                    var typeDefinitionTypeInfo = interfaceType
                        .GetGenericTypeDefinition()
                        .GetTypeInfo();

                    if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
                    {
                        return true;
                    }
                }
            }

            if (typeInfo.IsGenericType)
            {
                var typeDefinitionTypeInfo = typeInfo
                    .GetGenericTypeDefinition()
                    .GetTypeInfo();

                if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
                {
                    return true;
                }
            }

            var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();

            if (baseTypeInfo == null)
            {
                return false;
            }

            return baseTypeInfo.IsAssignableToGenericTypeDefinition(genericTypeInfo);
        }

        private static IEnumerable<Type> GetImplementedInterfacesToMap(TypeInfo typeInfo)
        {
            if (!typeInfo.IsGenericType)
            {
                return typeInfo.ImplementedInterfaces;
            }

            if (!typeInfo.IsGenericTypeDefinition)
            {
                return typeInfo.ImplementedInterfaces;
            }

            return FilterMatchingGenericInterfaces(typeInfo);
        }

        private static IEnumerable<Type> FilterMatchingGenericInterfaces(TypeInfo typeInfo)
        {
            var genericTypeParameters = typeInfo.GenericTypeParameters;

            foreach (var current in typeInfo.ImplementedInterfaces)
            {
                var currentTypeInfo = current.GetTypeInfo();

                if (currentTypeInfo.IsGenericType && currentTypeInfo.ContainsGenericParameters
                    && GenericParametersMatch(genericTypeParameters, currentTypeInfo.GenericTypeArguments))
                {
                    yield return currentTypeInfo.GetGenericTypeDefinition();
                }
            }
        }

        private static bool GenericParametersMatch(IReadOnlyList<Type> parameters, IReadOnlyList<Type> interfaceArguments)
        {
            if (parameters.Count != interfaceArguments.Count)
            {
                return false;
            }

            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] != interfaceArguments[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static MemberExpression ExtractMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return ((MemberExpression)expression);
            }

            if (expression.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression).Operand;
                return ExtractMemberExpression(operand);
            }

            return null;
        }

        #endregion Private
    }
}