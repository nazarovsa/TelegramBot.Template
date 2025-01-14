using ProjectName.Domain;
using ProjectName.Domain.Users;

namespace ProjectName.AppServices.Handlers;

public abstract class ContextHandlerBase
{
    protected readonly IUnitOfWork UnitOfWork;
    public UserAggregate User { get; private set; }

    protected ContextHandlerBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task SetUserContext(long telegramId, CancellationToken cancellationToken = default)
    {
        var user = await UnitOfWork.UsersRepository.GetById(telegramId, cancellationToken);

        User = user ?? throw new InvalidOperationException($"User not found: {telegramId}");
    }
}