using ProjectName.Domain.Users;
using ProjectName.Persistence.Infrastructure;

namespace ProjectName.Persistence.Repositories;

public  class UsersRepository : EfRepositoryBase<UserAggregate, ProjectNameDbContext>, IUsersRepository
{
    public UsersRepository(ProjectNameDbContext context) : base(context)
    {
    }
}