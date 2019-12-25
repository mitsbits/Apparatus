using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.EF.Data
{
    public class PlatformDbContextFactory : IDesignTimeDbContextFactory<PlatformDb>
    {
        private const string sqlConnectionString = "Server=localhost;Database=Borg;Trusted_Connection=True;MultipleActiveResultSets=true;";
        public PlatformDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlatformDb>();
            builder.UseSqlServer(sqlConnectionString, (o) =>
            {

            });
            return new PlatformDb(builder.Options);
        }
    }
}
