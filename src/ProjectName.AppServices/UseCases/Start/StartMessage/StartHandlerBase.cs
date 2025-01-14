using Insight.Localizer;
using Insight.TelegramBot;
using Insight.TelegramBot.Models;
using ProjectName.AppServices.Handlers;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectName.AppServices.UseCases.Start.StartMessage;

public abstract class StartHandlerBase : ContextHandlerBase
{
    private readonly ILocalizer _localizer;
    private readonly ITelegramBotClient _botClient;
    private readonly TimeProvider _timeProvider;

    public StartHandlerBase(
        ILocalizer localizer,
        ITelegramBotClient botClient,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider) : base(unitOfWork)
    {
        _localizer = localizer;
        _botClient = botClient;
        _timeProvider = timeProvider;
    }

    public async Task Handle(User tgUser, CancellationToken cancellationToken = default)
    {
        await SetUserContext(tgUser.Id, cancellationToken);
        Localizer.CurrentCulture = User.Culture;

        var userId = tgUser.Id;

        var message = new TextMessage(tgUser.Id)
        {
            Text = _localizer.Get(nameof(StartHandlerBase), "Text"),
            ReplyMarkup = CreateMainMenu(userId)
        };

        var user = await UnitOfWork.UsersRepository.GetById(userId, cancellationToken);
        if (user != null)
        {
            user.UpdateState(UserState.None, _timeProvider.GetUtcNow());
            await UnitOfWork.CommitAsync(cancellationToken);
        }

        await _botClient.SendMessage(message, cancellationToken);
    }

    private InlineKeyboardMarkup CreateMainMenu(long userId)
    {
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    _localizer.Get(nameof(StartHandlerBase), "ViewSettingsButton"),
                    new BotData(BotState.ViewSettings))
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }
}