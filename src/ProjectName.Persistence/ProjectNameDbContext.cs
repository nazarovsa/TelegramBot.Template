using Microsoft.EntityFrameworkCore;
using ProjectName.Domain.Users;
using ProjectName.Persistence.Configurations;

namespace ProjectName.Persistence;

public class ProjectNameDbContext : DbContext
{
    public DbSet<UserAggregate> Users { get; set; }

    public ProjectNameDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserAggregateConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}