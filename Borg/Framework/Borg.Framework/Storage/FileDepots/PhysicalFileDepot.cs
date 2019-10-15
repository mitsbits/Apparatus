using Borg.Framework.Storage.Contracts;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.FileDepots
{
    public class PhysicalFileDepot : PhysicalFileProvider, IFileDepot
    {
        public PhysicalFileDepot(string root, ExclusionFilters exclusionFilters = ExclusionFilters.Sensitive) : base(root, exclusionFilters)
        {
            
        }

        public Task Delete(string path, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            File.Delete(Path.Combine(Root, path));
            return Task.CompletedTask;
        }

        public Task<bool> Exists(string path, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(File.Exists(Path.Combine(Root, path)));
        }

        public async Task<IFileInfo> Save(string path, Stream stream, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var filepath = Path.Combine(Root, path);
            new FileInfo(filepath).Directory.Create();
            await stream.CopyToAsync(new FileStream(filepath, FileMode.Create));
            return GetFileInfo(filepath);
        }
    }
}
