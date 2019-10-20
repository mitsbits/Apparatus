using Borg.Infrastructure.Core.Exceptions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Borg.Infrastructure.Core
{
    [DebuggerStepThrough]
    internal static partial class Preconditions
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException("String value cannot be null.", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static DateTime NotEmpty(DateTime value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            if (value.Equals(default(DateTime)))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static TEnum IsDefined<TEnum>(TEnum value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "") where TEnum : struct
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static int PositiveOrZero([NoEnumeration] int value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            parameterName = NotEmpty(parameterName, nameof(parameterName));
            if (ReferenceEquals(value, null))
            {
                throw new IndexOutOfRangeException(parameterName);
            }

            if (value < 0)
            {
                throw new IndexOutOfRangeException($"Int value cannot be less than zero. {parameterName}");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static int Positive([NoEnumeration] int value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            parameterName = NotEmpty(parameterName, nameof(parameterName));
            if (ReferenceEquals(value, null))
            {
                throw new IndexOutOfRangeException(parameterName);
            }

            if (value <= 0)
            {
                throw new IndexOutOfRangeException($"Int value cannot be less or equal to zero. {parameterName}");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static int NegativeOrZero([NoEnumeration] int value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            parameterName = NotEmpty(parameterName, nameof(parameterName));
            if (ReferenceEquals(value, null))
            {
                throw new IndexOutOfRangeException(parameterName);
            }

            if (value > 0)
            {
                throw new IndexOutOfRangeException($"Int value cannot be more than zero. {parameterName}");
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "")
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Count() < 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException("Collection must have one ore more elements.", parameterName);
            }

            return value;
        }
    }

    internal static partial class Preconditions
    {
        public static T SubclassOf<T>([NotNull]Type target, [NoEnumeration, NotNull] T value, [InvokerParameterName, NotNull] string parameterName, [CallerMemberName] string callerName = "") where T : class
        {
            var typetocheck = value.GetType();
            if (typetocheck.IsSubclassOf(target))
            {
                return value;
            }

            throw new NotSubclassOfException(typetocheck, target, parameterName);
        }
    }
}