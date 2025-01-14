using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using ProjectName.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectName.AppServices.UseCases.Start.StartMessage.StartCallbackQuery;

public  class StartCallbackQueryHandler : StartHandlerBase, IMatchingUpdateHandler<StartCallbackQueryMatcher>
{
    public StartCallbackQueryHandler(ILocalizer localizer, ITelegramBotClient botClient, IUnitOfWork unitOfWork,
        TimeProvider timeProvider) : base(localizer, botClient, unitOfWork, timeProvider)
    {
    }

    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var user = update.CallbackQuery.From;
        return Handle(user, cancellationToken);
    }
}