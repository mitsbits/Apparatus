using Borg.Infrastructure.Core.Exceptions;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Text;

namespace Borg.Framework.Reflection.Exceptions
{
    public class NotAssimilatedException : BorgException
    {
        public NotAssimilatedException(AssemblyScanResult result, Exception exception = null) : base(GenerateMessage(result, exception), exception)
        {
        }

        private static string GenerateMessage(AssemblyScanResult result, Exception exception)
        {
            var builder = new StringBuilder(result.Assembly.FullName);
            builder.AppendLine($"Outcome {result.Success}");
            foreach (var error in result.Errors)
            {
                builder.AppendLine(error);
            }
            if (exception != null)
            {
                builder.AppendLine(exception.GetType().Name);
                builder.AppendLine(exception.Message);
            }

            return builder.ToString();
        }
    }
}