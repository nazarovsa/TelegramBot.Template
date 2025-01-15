using System.Globalization;
using FormatWith;
using Insight.Localizer;
using Insight.TelegramBot;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Models;
using Microsoft.Extensions.Logging;
using ProjectName.AppServices.Handlers;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectName.AppServices.UseCases.SetUserTimezone.SetUserTimeZoneAwaiting;

public class SetTimezoneAwaitingHandler : ContextHandlerBase, IContextMatchingUpdateHandler<SetTimezoneAwaitingMatcher>
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILocalizer _localizer;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SetTimezoneAwaitingHandler> _logger;

    public SetTimezoneAwaitingHandler(
        ITelegramBotClient botClient,
        ILocalizer localizer,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ILogger<SetTimezoneAwaitingHandler> logger) : base(unitOfWork)
    {
        _botClient = botClient;
        _localizer = localizer;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        await SetUserContext(update.Message.From.Id, cancellationToken);
        Localizer.CurrentCulture = User.Culture;

        if (!double.TryParse(update.Message.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var timezone))
        {
            var message = new TextMessage(update.Message.Chat.Id)
            {
                Text = _localizer.Get(nameof(SetTimezoneAwaitingHandler), "InvalidFormat"),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.SendMessage(message, cancellationToken);
            return;
        }

        if (timezone < -12 || timezone > 14)
        {
            var message = new TextMessage(update.Message.Chat.Id)
            {
                Text = _localizer.Get(nameof(SetTimezoneAwaitingHandler), "InvalidRange"),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.SendMessage(message, cancellationToken);
            return;
        }

        try
        {
            User.SetTimezone(timezone, _timeProvider.GetUtcNow());
            User.UpdateState(UserState.None, _timeProvider.GetUtcNow());
            await UnitOfWork.CommitAsync(cancellationToken);

            var successMessage = new TextMessage(update.Message.Chat.Id)
            {
                Text = _localizer.Get(nameof(SetTimezoneAwaitingHandler), "TimezoneUpdated")
                    .FormatWith(new { Timezone = timezone }),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.SendMessage(successMessage, cancellationToken);
            _logger.LogInformation("Successfully updated timezone to {Timezone} for user {UserId}",
                timezone, User.Id);
        }
        catch (Exception)
        {
            var message = new TextMessage(update.Message.Chat.Id)
            {
                Text = _localizer.Get(nameof(SetTimezoneAwaitingHandler), "Error"),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.SendMessage(message, cancellationToken);
            throw;
        }
    }
}