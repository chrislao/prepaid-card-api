using Microsoft.EntityFrameworkCore;
using PrepaidCardApi.Models;

namespace PrepaidCardApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PrepaidCard> Cards => Set<PrepaidCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrepaidCard>().ToTable("cards", "public");
    }
}