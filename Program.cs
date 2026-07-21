using Microsoft.EntityFrameworkCore;

namespace KabyliaTaste
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (var db = new KabyliaTaste.Data.AppDbContext())
            {
                // Ensure the migrations history table exists (created by EnsureCreated previously)
                db.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS __EFMigrationsHistory (
                        MigrationId TEXT NOT NULL CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY,
                        ProductVersion TEXT NOT NULL
                    )");

                // If the Products table already exists but InitialCreate was never recorded,
                // register it manually so Migrate() won't try to re-create it.
                var applied = db.Database
                    .SqlQueryRaw<string>("SELECT MigrationId FROM __EFMigrationsHistory")
                    .ToList();

                if (!applied.Contains("20260718164233_InitialCreate"))
                {
                    db.Database.ExecuteSqlRaw(
                        "INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) " +
                        "VALUES ('20260718164233_InitialCreate', '8.0.22')");
                }

                // Now only truly pending migrations (e.g. AddSale) will be applied
                    db.Database.Migrate();

                    // Seed default users if none exist
                    if (!db.Users.Any())
                    {
                        db.Users.Add(new KabyliaTaste.Models.User { Username = "admin", Password = "admin", IsAdmin = true });
                        db.Users.Add(new KabyliaTaste.Models.User { Username = "user", Password = "user", IsAdmin = false });
                        db.SaveChanges();
                    }

                    // Seed default store settings if none exist
                    if (!db.StoreSettings.Any())
                    {
                        db.StoreSettings.Add(new KabyliaTaste.Models.StoreSettings { StoreName = "KabyliaTaste" });
                        db.SaveChanges();
                    }
                }

                // Show login; loop so logout brings back the login screen
                while (true)
                {
                    Session.CurrentUser = null;
                    using var loginForm = new LoginForm();
                    if (loginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        break;

                    using var main = new Main();
                    Application.Run(main);

                    // If the user didn't request logout, exit the app
                    if (!main.LogoutRequested)
                        break;
                }
        }
    }
}