using Insight.Localizer;
using Insight.TelegramBot.DependencyInjection.Infrastructure;
using Insight.TelegramBot.DependencyInjection.Polling;
using Insight.TelegramBot.Handling.Infrastructure;
using Insight.TelegramBot.Polling.ExceptionHandlers;
using ProjectName.AppServices;
using ProjectName.AppServices.UseCases.Start.StartMessage;
using ProjectName.AppServices.UseCases.Start.StartMessage.StartMessage;
using ProjectName.Persistence;
using ProjectName.Persistence.Infrastructure;

namespace ProjectName.Host.Infrastructure;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var localizerConfiguration = Configuration
            .GetSection(nameof(LocalizerConfiguration))
            .Get<LocalizerConfiguration>();
        Localizer.Initialize(localizerConfiguration);
        services.AddSingleton<ILocalizer, Localizer>();
        
        services.AddAppServices(Configuration);
        services.AddEfPersistence(Configuration.GetConnectionString(nameof(ProjectNameDbContext))!);
        
        services.AddSingleton(TimeProvider.System);
        services.AddTelegramBot(bot =>
            bot.WithTelegramBotClient(client => client
                    .WithLifetime(ServiceLifetime.Singleton)
                    .WithMicrosoftHttpClientFactory())
                .WithOptions(opt => opt.FromConfiguration(Configuration))
                .WithPolling(polling => polling.WithExceptionHandler<LoggingPollingExceptionHandler>()));
        services.AddTelegramBotHandling(typeof(StartMessageHandler).Assembly);
        
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}