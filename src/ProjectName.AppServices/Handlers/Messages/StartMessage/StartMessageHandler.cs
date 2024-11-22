using Insight.Localizer;
using Insight.TelegramBot;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectName.AppServices.Handlers.Messages.StartMessage;

public class StartMessageHandler : IMatchingUpdateHandler<StartMessageMatcher>
{
    private readonly ILocalizer _localizer;
    private readonly ITelegramBotClient _botClient;

    public StartMessageHandler(ILocalizer localizer, ITelegramBotClient botClient)
    {
        _localizer = localizer;
        _botClient = botClient;
    }

    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var message = new TextMessage(update.Message.Chat.Id)
        {
            Text = _localizer.Get(nameof(StartMessageHandler), "Text")
        };

        return _botClient.SendMessage(message, cancellationToken);
    }
}