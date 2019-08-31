namespace Borg.Framework.EF.Contracts
{
    public interface IUOWDbContext
    {
    }

    public interface IBorgDbContextOptions
    {
        string OverrideSchema { get; }
        string TablePrefix { get; }
    }

    public class BorgDbContextOptions : IBorgDbContextOptions
    {
        public string OverrideSchema { get; set; } = string.Empty;
        public string TablePrefix { get; set; } = string.Empty;
    }
}