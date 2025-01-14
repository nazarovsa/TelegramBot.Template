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

namespace ProjectName.AppServices.UseCases.UserSettings.ViewSettings;

public class ViewSettingsHandler : ContextHandlerBase, IMatchingUpdateHandler<ViewSettingsMatcher>
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILocalizer _localizer;
    private readonly ILogger<ViewSettingsHandler> _logger;

    public ViewSettingsHandler(
        ITelegramBotClient botClient,
        ILocalizer localizer,
        IUnitOfWork unitOfWork,
        ILogger<ViewSettingsHandler> logger) : base(unitOfWork)
    {
        _botClient = botClient;
        _localizer = localizer;
        _logger = logger;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        await SetUserContext(update.CallbackQuery.From.Id, cancellationToken);
        Localizer.CurrentCulture = User.Culture;

        var (toggleToKey, toggleToArg) = User.Culture.Equals("ru")
            ? ("ToggleToEnglishButton", "en")
            : ("ToggleToRussianButton", "ru");

        var toggleToText = _localizer.Get(nameof(ViewSettingsHandler), toggleToKey);
        var message = new TextMessage(update.CallbackQuery.Message!.Chat.Id)
        {
            Text = _localizer.Get(nameof(ViewSettingsHandler), "SettingsMessage"),
            ReplyMarkup = new InlineKeyboardMarkup([
                [
                    InlineKeyboardButton.WithCallbackData(
                        toggleToText,
                        new BotData(BotState.ToggleLanguage, toggleToArg))
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        _localizer.GetBackButtonText(),
                        BotData.Start())
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