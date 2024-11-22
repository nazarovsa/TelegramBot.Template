using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectName.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectNameDbContext>
{
    private const string EnvironmentVariableKey = "DOTNET_ENVIRONMENT";

    public ProjectNameDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectNameDbContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var environment = configuration.GetValue<string>(EnvironmentVariableKey);
        if (string.IsNullOrWhiteSpace(environment))
        {
            throw new InvalidOperationException($"Отсутствует переменная окружения {EnvironmentVariableKey}");
        }

        var connectionString = configuration.GetConnectionString(environment);
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"ConnectionString for environment `{environment}` not found");

        optionsBuilder.UseMySql(connectionString, new MariaDbServerVersion(MariaDbServerVersion.LatestSupportedServerVersion),
            builder => builder.MigrationsAssembly("CodeReviewBot.Migrator"));

        return new ProjectNameDbContext(optionsBuilder.Options);
    }
}