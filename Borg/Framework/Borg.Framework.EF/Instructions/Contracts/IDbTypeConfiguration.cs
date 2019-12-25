using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.Instructions.Contracts
{
    public interface IDbTypeConfiguration
    {
        void ConfigureDb(ModelBuilder builder);
    }
}