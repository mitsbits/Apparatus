namespace Borg.Infrastructure.Core.Strings.Services
{
    public interface IJsonConverter
    {
        string Serialize(object ob);

        T DeSerialize<T>(string source);
    }
}