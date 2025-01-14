using Insight.TelegramBot.Handling.Handlers;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectName.AppServices.Handlers;

public class InitializeOrUpdateUserHandler : IUpdateHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public InitializeOrUpdateUserHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider)
    {
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        var tgUser = GetUser(update);
        var user = await _unitOfWork.UsersRepository.GetById(tgUser.Id, cancellationToken);
        var utcNow = _timeProvider.GetUtcNow();
        if (user == null)
        {
            var culture = tgUser.LanguageCode != null
                          && tgUser.LanguageCode.Equals("ru", StringComparison.OrdinalIgnoreCase)
                ? "ru"
                : "en";

            user = UserAggregate.Initialize(tgUser.Id, tgUser.FirstName, utcNow, culture, tgUser.Username);
            _unitOfWork.UsersRepository.Create(user);
            await _unitOfWork.CommitAsync(cancellationToken);
            return;
        }

        user.Update(tgUser.FirstName, utcNow, tgUser.Username);
        user.UpdateLastActivityAt(utcNow);

        _unitOfWork.UsersRepository.Update(user);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private User GetUser(Update update)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                return update.Message!.From;
            case UpdateType.CallbackQuery:
                return update.CallbackQuery!.From;
            default:
                throw new ArgumentOutOfRangeException(nameof(Update.Type));
        }
    }
}