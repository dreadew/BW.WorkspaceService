using Common.Base.Constants;
using Common.Services.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WorkspaceService.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(ConfigurationConstants.SettingsFileName)
            .Build();

        var extendedConfiguration = new ConfigurationBuilder()
            .AddVaultConfiguration(options =>
            {
                configuration.GetSection(VaultConstants.VaultSection).Bind(options);
            })
            .Build();
        
        optionsBuilder.UseNpgsql(extendedConfiguration[DbConstants.DefaultConnectionString]);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}