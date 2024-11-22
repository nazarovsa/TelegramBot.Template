using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using Telegram.Bot.Types;

namespace ProjectName.AppServices.Handlers;

public sealed class SetLocalizerCultureHandler : IUpdateHandler
{
    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        Localizer.CurrentCulture = "ru";
        return Task.CompletedTask;
    }
}