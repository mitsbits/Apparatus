//using Borg.Framework.Azure.Storage.Blobs.FileProviders;
//using Borg.Framework.Storage.Contracts;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using Microsoft.Extensions.Primitives;
//using Microsoft.WindowsAzure.Storage.Blob;
//using System;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Borg.Framework.Azure.Storage.Blobs.FileDepots
//{
//    public class AzureBlobFileDepot : IFileDepot
//    {
//        protected readonly ILogger _logger;
//        protected readonly IBlobContainerFactory _blobContainerFactory;
//        protected readonly CloudBlobContainer _cloudBlobContainer;

//        public AzureBlobFileDepot(ILoggerFactory loggerFactory, IBlobContainerFactory blobContainerFactory)
//        {
//            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
//            _blobContainerFactory = blobContainerFactory;
//            _cloudBlobContainer = _blobContainerFactory.GetContainer();
//        }

//        public AzureBlobFileDepot(AzureBlobOptions azureBlobOptions)
//        {
//            _blobContainerFactory = new AzureBlobContainerFactory(azureBlobOptions);
//            _cloudBlobContainer = _blobContainerFactory.GetContainer();
//        }

//        public virtual IDirectoryContents GetDirectoryContents(string subpath)
//        {
//            var blob = _cloudBlobContainer.GetDirectoryReference(_blobContainerFactory.TransformPath(subpath));
//            return new AzureBlobDirectoryContents(blob);
//        }

//        public virtual IFileInfo GetFileInfo(string subpath)
//        {
//            var blob = _cloudBlobContainer.GetBlockBlobReference(_blobContainerFactory.TransformPath(subpath));
//            return new AzureBlobFileInfo(blob);
//        }

//        public virtual IChangeToken Watch(string filter) => throw new NotImplementedException();

//        public virtual async Task<bool> Delete(string path, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();

//            var blob = _cloudBlobContainer.GetBlockBlobReference(_blobContainerFactory.TransformPath(path));
//            await blob.DeleteAsync();
//            return true;
//        }

//        public virtual async Task<bool> Exists(string path, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            var blob = _cloudBlobContainer.GetBlockBlobReference(_blobContainerFactory.TransformPath(path));
//            return await blob.ExistsAsync();
//        }

//        public virtual async Task<bool> Save(string path, Stream stream, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            var blockBlob = _cloudBlobContainer.GetBlockBlobReference(_blobContainerFactory.TransformPath(path));
//            blockBlob.Properties.ContentType = path.GetMimeType();
//            await blockBlob.UploadFromStreamAsync(stream).AnyContext();
//            return true;
//        }
//    }
//}