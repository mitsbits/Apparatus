using System;

namespace Borg.Infrastructure.Core.Exceptions
{
    public class NotSubclassOfException : BorgException
    {
        public NotSubclassOfException(Type typeToCheck, Type baseType, string parameterName = "") : base(ExceptionMessage(typeToCheck, baseType))
        {
            ParameterName = parameterName;
        }

        private static string ExceptionMessage(Type typeToCheck, Type baseType)
        {
            return $"{typeToCheck.FullName} is not sub class of {baseType.FullName}";
        }

        public string ParameterName { get; set; }
    }
}