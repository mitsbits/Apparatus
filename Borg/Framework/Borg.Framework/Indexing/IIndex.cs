using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Indexing
{
    public interface IIndex<out T> : IIndex where T : IIndexable
    {
        IQueryable<T> Documents { get; }
    }

    public interface IIndexWriter<in T> : IIndex where T : IIndexable
    {
        Task Index(T indexable);
    }
    public interface IIndex : IHaveName
    {

    }

    public interface IIndexField
    {

    }

    public interface IIndexable
    {

    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IndexFieldAttribute : Attribute
    {
        public IndexFieldAttribute(string indexFieldName)
        {
            IndexFieldName = Preconditions.NotEmpty(indexFieldName, nameof(indexFieldName));
        }
        public string IndexFieldName { get; }
    }
}
