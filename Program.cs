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

                var applied = db.Database
                    .SqlQueryRaw<string>("SELECT MigrationId FROM __EFMigrationsHistory")
                    .ToList();

                void MarkApplied(string migrationId)
                {
                    if (!applied.Contains(migrationId))
                    {
                        db.Database.ExecuteSqlRaw(
                            "INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ({0}, '8.0.22')",
                            migrationId);
                        applied.Add(migrationId);
                    }
                }

                bool TableExists(string tableName) =>
                    db.Database.SqlQueryRaw<string>(
                            "SELECT name FROM sqlite_master WHERE type='table' AND name={0}", tableName)
                        .ToList()
                        .Any();

                bool ColumnExists(string tableName, string columnName) =>
                    db.Database.SqlQueryRaw<string>(
                            $"SELECT name FROM pragma_table_info('{tableName}') WHERE name={{0}}", columnName)
                        .ToList()
                        .Any();

                // Only mark a migration as applied if its resulting schema is actually present.
                // This prevents Migrate() from re-running ALTER/CREATE statements for schema
                // changes that already exist in app.db (which caused the duplicate column error).
                if (TableExists("Products"))
                    MarkApplied("20260718164233_InitialCreate");

                if (TableExists("Sales"))
                    MarkApplied("20260719063145_AddSale");

                if (TableExists("Invoices") && ColumnExists("Sales", "InvoiceId"))
                    MarkApplied("20260803132253_AddInvoices");

                // Now only truly pending migrations will be applied
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