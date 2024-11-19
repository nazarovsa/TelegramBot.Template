using Insight.TelegramBot.Handling.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectName.AppServices.Telegram.Handlers;

namespace ProjectName.AppServices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) 
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IUpdateHandler, SetLocalizerCultureHandler>();
        
        return services;
    }
}