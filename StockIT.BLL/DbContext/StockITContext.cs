using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace StockIT.Models;

public class StockITContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure connection string to your database here
        optionsBuilder.UseSqlServer("Server=tcp:oracleconsults.database.windows.net,1433;Initial Catalog=StockIt;Persist Security Info=False;User ID=oreste.twizeyimana;Password=Raphael47@Microsoft;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            );
    }
    public StockITContext(DbContextOptions<StockITContext> options) : base(options)
    {
    }
}
