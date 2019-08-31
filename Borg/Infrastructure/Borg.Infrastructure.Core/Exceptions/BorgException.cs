using System;

namespace Borg.Infrastructure.Core.Exceptions
{

    public abstract class BorgException : Exception
    {
        protected BorgException()
        {
        }

        protected BorgException(string message)
            : base(message)
        {
        }

        protected BorgException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public Guid ExceptionId { get; } = Guid.NewGuid();
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    }
}