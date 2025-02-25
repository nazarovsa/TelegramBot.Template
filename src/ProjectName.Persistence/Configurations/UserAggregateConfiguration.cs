using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectName.Domain.Users;
using ProjectName.Persistence.Infrastructure;

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

        builder.Property(x => x.Culture)
            .HasColumnName("culture")
            .HasDefaultValue("en")
            .HasMaxLength(2);

        builder.Property(x => x.Timezone)
            .HasColumnName("timezone")
            .IsRequired();

        builder.Property(x => x.State)
            .HasColumnName("state")
            .HasColumnType("int");

        builder.Property(x => x.StateData)
            .HasColumnName("state_data")
            .HasColumnType("json")
            .HasField("_stateData")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasJsonConversion();

        builder.Property(x => x.LastActivityAt)
            .HasColumnName("last_activity_at");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");

        // Indexes
        builder.HasIndex(x => x.Id)
            .IsUnique()
            .HasDatabaseName("ix_users_telegram_id");

        builder.HasIndex(x => x.LastActivityAt)
            .HasDatabaseName("ix_users_last_activity_at");
    }
}