using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Models;
using Microsoft.Extensions.Logging;
using ProjectName.AppServices.Extensions;
using ProjectName.AppServices.Handlers;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectName.AppServices.UseCases.SetUserTimezone;

public class SetTimezoneHandler : ContextHandlerBase, IMatchingUpdateHandler<SetTimezoneMatcher>
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILocalizer _localizer;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SetTimezoneHandler> _logger;

    public SetTimezoneHandler(
        ITelegramBotClient botClient,
        ILocalizer localizer,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ILogger<SetTimezoneHandler> logger) : base(unitOfWork)
    {
        _botClient = botClient;
        _localizer = localizer;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        await SetUserContext(update.CallbackQuery.From.Id, cancellationToken);
        Localizer.CurrentCulture = User.Culture;

        User.UpdateState(UserState.SetTimezoneAwaiting, _timeProvider.GetUtcNow());
        await UnitOfWork.CommitAsync(cancellationToken);

        var message = new TextMessage(update.CallbackQuery.Message!.Chat.Id)
        {
            Text = _localizer.Get(nameof(SetTimezoneHandler), "SetTimezoneAwaitingText"),
            ReplyMarkup = new InlineKeyboardMarkup([
                [
                    InlineKeyboardButton.WithCallbackData(
                        _localizer.GetBackButtonText(),
                        new BotData(BotState.ViewSettings))
                ]
            ])
        };

        await _botClient.EditOrSendTextMessage(
            update.CallbackQuery.Message.MessageId,
            message,
            _logger,
            cancellationToken);
    }
}