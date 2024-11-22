using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using ProjectName.Persistence.Repositories;

namespace ProjectName.Persistence.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfPersistence(this IServiceCollection services, string connectionString)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        services.AddScoped<IUsersRepository, UsersRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ProjectNameDbContext>(
            (ctx, context) =>
            {
                context.UseMySql(connectionString,
                        new MariaDbServerVersion(MariaDbServerVersion.LatestSupportedServerVersion),
                        builder => builder.MigrationsAssembly(typeof(ProjectNameDbContext).Assembly.FullName))
                    .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>());
            }
        );

        return services;
    }
}