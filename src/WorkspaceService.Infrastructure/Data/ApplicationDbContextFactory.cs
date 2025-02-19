using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Excpetions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Infrastructure.Services;

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
        
        ISecretsProvider secretsProvider = new InfiscalSecretsProvider(configuration);
        
        string? connectionString = secretsProvider.GetSecret(
            DBConstants.DefaultConnectionString,
            "dev");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new VariableNotFoundException("Ошибка при создании клиента БД",
                nameof(connectionString),
                nameof(ApplicationDbContextFactory));
        }
        
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}