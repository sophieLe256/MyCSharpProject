namespace WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

// DbContext đại diện cho một phiên làm việc với cơ sở dữ liệu, 
//nó cho phép chúng ta truy vấn và lưu dữ liệu vào database
public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasPrecision(10, 2);
    }
}
