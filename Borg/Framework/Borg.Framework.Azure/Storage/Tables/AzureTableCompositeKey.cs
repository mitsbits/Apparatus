using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Framework.Azure.Storage.Tables
{
    public class AzureTableCompositeKey : CompositeKey
    {
        public AzureTableCompositeKey() : base()
        {
        }

        public AzureTableCompositeKey(string partition, string row) : base(
            new List<(string key, object value)>(new[] { (partition: partition, row: row as object) }))
        {
        }

        public override void Add(string key, object value)
        {
            key = Preconditions.NotEmpty(key, nameof(key));
            value = Preconditions.NotNull(value, nameof(key));
            base.Add(key, value);
        }

        public string Partition => _data.FirstOrDefault(x => x.Key == nameof(Partition)).Value.ToString();
        public string Row => _data.FirstOrDefault(x => x.Key == nameof(Row)).Value.ToString();

        public static AzureTableCompositeKey Create(string partition, string row)
        {
            return new AzureTableCompositeKey(partition, row);
        }
    }
}