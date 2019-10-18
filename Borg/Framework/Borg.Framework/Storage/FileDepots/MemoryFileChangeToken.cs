using Borg.Framework.Storage.FileProviders;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using System.Threading;

namespace Borg.Framework.Storage.FileDepots
{
    public partial class MemoryFileDepot
    {
        public class MemoryFileChangeToken : IChangeToken
        {
            private readonly MemoryFileInfo _fileInfo;
            private DateTime _previousWriteTimeUtc;
            private DateTime _lastCheckedTimeUtc;
            private bool _hasChanged;
            private FileOperation _fileOperation;
            private CancellationTokenSource _tokenSource;
            private CancellationChangeToken _changeToken;

            internal CancellationTokenSource CancellationTokenSource
            {
                get => _tokenSource;
                set
                {
                    Debug.Assert(_tokenSource == null, "We expect CancellationTokenSource to be initialized exactly once.");

                    _tokenSource = value;
                    _changeToken = new CancellationChangeToken(_tokenSource.Token);
                }
            }

            public MemoryFileChangeToken(MemoryFileInfo fileInfo)
            {
                _fileInfo = Preconditions.NotNull(fileInfo, nameof(fileInfo));
                _previousWriteTimeUtc = GetLastWriteTimeUtc();
            }

            private DateTime GetLastWriteTimeUtc()
            {
                return _fileInfo.Exists ? _fileInfo.LastWriteTimeUtc : DateTime.MinValue;
            }

            public void Change(FileOperation fileOperation)
            {
                if (_hasChanged) return;
                _fileOperation = fileOperation;
                _hasChanged = true;
            }

            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                if (!ActiveChangeCallbacks)
                {
                    return EmptyDisposable.Instance;
                }

                return _changeToken.RegisterChangeCallback(callback, state);
            }

            public bool HasChanged
            {
                get
                {
                    return _hasChanged;
                }
            }

            public bool ActiveChangeCallbacks => false;
        }
    }