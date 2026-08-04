using Microsoft.EntityFrameworkCore;
using KabyliaTaste.Models;

namespace KabyliaTaste.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<StoreSettings> StoreSettings { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var appDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "KabyliaTaste");

                Directory.CreateDirectory(appDataFolder);

                optionsBuilder.UseSqlite($"Data Source={Path.Combine(appDataFolder, "app.db")}");
            }
        }
    }
}
