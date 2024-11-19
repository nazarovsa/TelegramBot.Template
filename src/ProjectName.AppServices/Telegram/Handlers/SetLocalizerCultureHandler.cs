using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using Telegram.Bot.Types;

namespace ProjectName.AppServices.Telegram.Handlers;

public sealed class SetLocalizerCultureHandler : IUpdateHandler
{
    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        Localizer.CurrentCulture = "ru-ru";
        return Task.CompletedTask;
    }
}