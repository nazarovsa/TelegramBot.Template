using Insight.TelegramBot.Handling.Matchers;
using ProjectName.Domain;
using ProjectName.Domain.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectName.AppServices.UseCases.SetUserTimezone.SetUserTimeZoneAwaiting;

public class SetTimezoneAwaitingMatcher : IContextUpdateMatcher
{
    private readonly IUnitOfWork _unitOfWork;

    public SetTimezoneAwaitingMatcher(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> MatchesAsync(Update update, CancellationToken cancellationToken = default)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text == null)
            return false;

        var user = await _unitOfWork.UsersRepository.GetById(update.Message.From.Id, cancellationToken);
        return user is { State: UserState.SetTimezoneAwaiting };
    }
}