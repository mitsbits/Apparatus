namespace Borg.Framework.Azure.Storage.Tables
{
    public interface IHaveTableKey
    {
        AzureTableCompositeKey Key { get; }
    }
}