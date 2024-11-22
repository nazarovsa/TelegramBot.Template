using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectName.Domain.Users;

namespace ProjectName.Persistence.Configurations;

public class UserAggregateConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100);

        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasMaxLength(100);

        builder.Property(x => x.LastActivityAt)
            .HasColumnName("last_activity_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        
        builder.HasIndex(x => x.LastActivityAt)
            .HasDatabaseName("ix_users_last_activity_at");
    }
}