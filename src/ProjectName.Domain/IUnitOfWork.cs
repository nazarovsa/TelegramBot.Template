using ProjectName.Domain.Users;

namespace ProjectName.Domain;

public interface IUnitOfWork
{
    IUsersRepository UsersRepository { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
}