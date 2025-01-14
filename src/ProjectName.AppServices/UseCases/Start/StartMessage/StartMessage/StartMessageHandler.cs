using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using ProjectName.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectName.AppServices.UseCases.Start.StartMessage.StartMessage;

public  class StartMessageHandler : StartHandlerBase, IMatchingUpdateHandler<StartMessageMatcher>
{
    public StartMessageHandler(ILocalizer localizer, ITelegramBotClient botClient, IUnitOfWork unitOfWork,
        TimeProvider timeProvider) : base(localizer, botClient, unitOfWork, timeProvider)
    {
    }

    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var user = update.Message.From;
        return Handle(user, cancellationToken);
    }
}