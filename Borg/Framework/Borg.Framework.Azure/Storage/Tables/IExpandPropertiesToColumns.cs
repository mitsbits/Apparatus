using Microsoft.WindowsAzure.Storage.Table;

namespace Borg.Framework.Azure.Storage.Tables
{
    public interface IExpandPropertiesToColumns
    {
        ITableEntity Expanded();
    }
}