using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eLog.HeavyTools.Services.WhZone.DataAccess.Context
{
    public class WhZoneDbContextFactory : IDesignTimeDbContextFactory<WhZoneDbContext>
    {
        public WhZoneDbContext CreateDbContext(string[] args)
        {
            var basePath = AppContext.BaseDirectory;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<WhZoneDbContext>();
            var connectionString = configuration.GetConnectionString(nameof(WhZoneDbContext));
            optionsBuilder.UseSqlServer(connectionString);

            return new WhZoneDbContext(optionsBuilder.Options);
        }
    }
}
