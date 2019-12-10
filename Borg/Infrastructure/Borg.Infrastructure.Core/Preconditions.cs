using Borg.Infrastructure.Core.Exceptions;
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
        public static T NotNull<T>(T value, string parameterName, [CallerMemberName] string callerName = "")
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName, [CallerMemberName] string callerName = "")
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

        public static DateTime NotEmpty(DateTime value, string parameterName, [CallerMemberName] string callerName = "")
        {
            if (value.Equals(default(DateTime)))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static TEnum IsDefined<TEnum>(TEnum value, string parameterName, [CallerMemberName] string callerName = "") where TEnum : struct
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        public static int PositiveOrZero(int value, string parameterName, [CallerMemberName] string callerName = "")
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

        public static int Positive(int value, string parameterName, [CallerMemberName] string callerName = "")
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

        public static int NegativeOrZero(int value, string parameterName, [CallerMemberName] string callerName = "")
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

        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> value, string parameterName, [CallerMemberName] string callerName = "")
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
        public static T SubclassOf<T>(Type target, T value, string parameterName, [CallerMemberName] string callerName = "") where T : class
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