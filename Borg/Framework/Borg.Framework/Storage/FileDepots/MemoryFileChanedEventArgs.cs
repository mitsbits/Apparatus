using System;

namespace Borg.Framework.Storage.FileDepots
{
    public partial class MemoryFileDepot
    {
        public class MemoryFileChanedEventArgs : EventArgs
        {
            internal MemoryFileChanedEventArgs(FileOperation fileOperation, string path)
            {
                FileOperation = fileOperation;
                Path = path;
            }

            internal FileOperation FileOperation { get; }
            internal string Path { get; }
        }
    }