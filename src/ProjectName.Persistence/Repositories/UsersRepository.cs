using ProjectName.Domain.Users;
using ProjectName.Persistence.Infrastructure;

namespace ProjectName.Persistence.Repositories;

public sealed class UsersRepository : EfRepositoryBase<UserAggregate, ProjectNameDbContext>, IUsersRepository
{
    public UsersRepository(ProjectNameDbContext context) : base(context)
    {
    }
}