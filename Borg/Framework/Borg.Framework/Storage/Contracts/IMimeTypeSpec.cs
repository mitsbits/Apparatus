namespace Borg.Framework.Storage.Contracts
{
    public interface IMimeTypeSpec
    {
        string Extension { get; }
        string MimeType { get; }
    }
}