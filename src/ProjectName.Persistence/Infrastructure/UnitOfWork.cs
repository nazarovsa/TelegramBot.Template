using ProjectName.Domain;
using ProjectName.Domain.Users;

namespace ProjectName.Persistence.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ProjectNameDbContext _context;
    
    private bool _disposed = false;

    public IUsersRepository UsersRepository { get; }
    
    public UnitOfWork(
        ProjectNameDbContext context,
        IUsersRepository usersRepository)
    {
        _context = context;
        UsersRepository = usersRepository;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}