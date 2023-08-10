using FinanceBot.DbSettings.ORM;
using Microsoft.EntityFrameworkCore;


namespace FinanceBot.DbSettings;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Income> Incomes { get; set; } = null!;
    public ApplicationContext() => Database.EnsureCreated();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
        
    }
}