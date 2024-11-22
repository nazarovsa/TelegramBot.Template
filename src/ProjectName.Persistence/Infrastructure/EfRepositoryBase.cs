using Microsoft.EntityFrameworkCore;
using ProjectName.Domain;

namespace ProjectName.Persistence.Infrastructure;

public abstract class EfRepositoryBase<TAggregate, TDbContext> : IRepository<TAggregate>
    where TAggregate : class
    where TDbContext : DbContext
{
    protected TDbContext Context { get; }

    protected DbSet<TAggregate> DbSet { get; }

    protected EfRepositoryBase(TDbContext context)
    {
        Context = context;
        DbSet = context.Set<TAggregate>();
    }

    public virtual void Create(TAggregate aggregate)
    {
        DbSet.Add(aggregate);
    }

    public virtual ValueTask<TAggregate?> GetById(long id, CancellationToken cancellationToken = default)
    {
        return DbSet.FindAsync(id, cancellationToken);
    }

    public virtual void Update(TAggregate aggregate)
    {
        DbSet.Attach(aggregate);
        DbSet.Entry(aggregate).State = EntityState.Modified;
    }
}