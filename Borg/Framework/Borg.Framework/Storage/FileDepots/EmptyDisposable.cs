using System;

namespace Borg.Framework.Storage.FileDepots
{
    public partial class MemoryFileDepot
    {
        internal class EmptyDisposable : IDisposable
        {
            public static EmptyDisposable Instance { get; } = new EmptyDisposable();

            private EmptyDisposable()
            {
            }

            public void Dispose()
            {
            }
        }
    }