using Borg.Framework.Indexing;
using Borg.Infrastructure.Core;
using Lucene.Net.Index;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Analysis;

namespace Borg.Platform.Indexing.Lucene
{
    internal class Index<T> : IIndexWriter<T> where T : IIndexable
    {
        private const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;
        private readonly string path;
        private readonly IndexWriter indexWriter;
        protected Index(string path)
        {
            this.path = Preconditions.NotEmpty(path, nameof(path));
            var dir = FSDirectory.Open(this.path);
            //var analyzer = new Analyzer();
            //this.indexWriter = new IndexWriter(dir, new IndexWriterConfig(MATCH_LUCENE_VERSION, new StandardAnalyzer(MATCH_LUCENE_VERSION)));
        }
        public string Name => throw new NotImplementedException();

        Task IIndexWriter<T>.Index(T indexable)
        {
            throw new NotImplementedException();
        }
    }
}
