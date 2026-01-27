using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Saturn.Messages;

namespace Saturn.Types;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<ToroUser> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToroUser>()
            .Property(u => u.PropertyList)
            .HasConversion(
                v => JsonSerializer.Serialize(v),
                v => JsonSerializer.Deserialize<List<UserProperty>>(v)
            );
    }
}
