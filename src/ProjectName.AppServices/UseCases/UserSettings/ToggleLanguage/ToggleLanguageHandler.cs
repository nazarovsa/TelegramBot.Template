using FormatWith;
using Insight.Localizer;
using Insight.TelegramBot.Handling.Handlers;
using Insight.TelegramBot.Models;
using Microsoft.Extensions.Logging;
using ProjectName.AppServices.Extensions;
using ProjectName.AppServices.Handlers;
using ProjectName.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectName.AppServices.UseCases.UserSettings.ToggleLanguage;

public class ToggleLanguageHandler : ContextHandlerBase, IMatchingUpdateHandler<ToggleLanguageMatcher>
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILocalizer _localizer;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<ToggleLanguageHandler> _logger;

    public ToggleLanguageHandler(
        ITelegramBotClient botClient,
        ILocalizer localizer,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ILogger<ToggleLanguageHandler> logger) : base(unitOfWork)
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

        try
        {
            // Toggle between en and ru
            var callbackData = BotData.Parse(update.CallbackQuery.Data);
            var newCulture = callbackData.Args.First();
            User.UpdateCulture(newCulture, _timeProvider.GetUtcNow());
            await UnitOfWork.CommitAsync(cancellationToken);
            Localizer.CurrentCulture = newCulture;

            // Send success message with timing and new culture
            var message = new TextMessage(update.CallbackQuery.Message!.Chat.Id)
            {
                Text = _localizer.Get(nameof(ToggleLanguageHandler), "LanguageChanged")
                    .FormatWith(new { Culture = newCulture.ToUpper() }),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.EditOrSendTextMessage(
                update.CallbackQuery.Message.MessageId,
                message,
                _logger,
                cancellationToken);

            _logger.LogInformation("Successfully changed language to {Culture} for user {UserId}", 
                newCulture, User.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to toggle language for user {UserId}", User.Id);
            
            var errorMessage = new TextMessage(update.CallbackQuery.Message!.Chat.Id)
            {
                Text = _localizer.Get(nameof(ToggleLanguageHandler), "LanguageChangeError"),
                ReplyMarkup = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithCallbackData(
                            _localizer.Get("Buttons", "BackToSettings"),
                            new BotData(BotState.ViewSettings))
                    ]
                ])
            };

            await _botClient.EditOrSendTextMessage(
                update.CallbackQuery.Message.MessageId,
                errorMessage,
                _logger,
                cancellationToken);
        }
    }
}