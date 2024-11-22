using Microsoft.EntityFrameworkCore;

namespace ProjectName.Persistence;

public class Program
{
    public static async Task Main(string[] args)
    {
        var contextFactory = new DesignTimeDbContextFactory();
        using (var context = contextFactory.CreateDbContext([]))
        {
            await context.Database.MigrateAsync();
            await context.SaveChangesAsync();
            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            Console.WriteLine("Applied migrations:");
            foreach (var appliedMigration in appliedMigrations)
            {
                Console.WriteLine(appliedMigration);
            }
        }
    }
}