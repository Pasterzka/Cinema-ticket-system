using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shared.Database
{
    public class CineQueueDbContextFactory : IDesignTimeDbContextFactory<CineQueueDbContext>
    {
        public CineQueueDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CineQueueDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CineQueueDb;Username=admin;Password=admin");

            return new CineQueueDbContext(optionsBuilder.Options);
        }
    }
}