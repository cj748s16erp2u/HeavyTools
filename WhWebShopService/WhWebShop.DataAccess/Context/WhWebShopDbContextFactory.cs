using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;

public class WhWebShopDbContextFactory : IDesignTimeDbContextFactory<WhWebShopDbContext>
{
    public WhWebShopDbContext CreateDbContext(string[] args)
    {
        var basePath = AppContext.BaseDirectory;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<WhWebShopDbContext>();
        var connectionString = configuration.GetConnectionString(nameof(WhWebShopDbContext));
        optionsBuilder.UseSqlServer(connectionString);

        return new WhWebShopDbContext(optionsBuilder.Options);
    }
}
