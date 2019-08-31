using Borg.Infrastructure.Core.Exceptions;
using System;

namespace Borg.Platform.EF.Exceptions
{
    public abstract class BorgEfException : BorgException
    {
        protected BorgEfException()
        {
        }

        protected BorgEfException(string message)
            : base(message)
        {
        }

        protected BorgEfException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}