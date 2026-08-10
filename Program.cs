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
            try
            {
                ApplicationConfiguration.Initialize();
                Environment.CurrentDirectory = AppContext.BaseDirectory;

                var licenseService = new KabyliaTaste.Services.StoreLicenseService();
                var license = licenseService.CheckLicenseAsync().GetAwaiter().GetResult();

                if (license.IsPackagedApp && !license.IsLicenseValid)
                {
                    var message = license.ErrorMessage ?? "Une licence Microsoft Store valide est introuvable.";
                    MessageBox.Show(message, "Amiar Store Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                                "SELECT name FROM pragma_table_info({0}) WHERE name={1}", tableName, columnName)
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

                    if (ColumnExists("StoreSettings", "CurrencyCode") &&
                        ColumnExists("StoreSettings", "LanguageCode") &&
                        ColumnExists("StoreSettings", "ProductUnitsJson") &&
                        ColumnExists("Products", "UnitName"))
                    {
                        MarkApplied("20260808151006_AddStorePreferencesAndCustomProductUnits");
                    }

                    if (ColumnExists("StoreSettings", "CurrencyCode") &&
                        ColumnExists("StoreSettings", "LanguageCode") &&
                        ColumnExists("StoreSettings", "ProductUnitsJson"))
                    {
                        MarkApplied("20260808170000_AddStoreSettingsPreferenceColumns");
                    }

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
                        db.StoreSettings.Add(new KabyliaTaste.Models.StoreSettings { StoreName = "Amiar Store Manager" });
                        db.SaveChanges();
                    }

                    var languageCode = db.StoreSettings.Select(s => s.LanguageCode).FirstOrDefault();
                    KabyliaTaste.Services.AppLocalization.SetLanguage(languageCode);
                }

                // Show login; loop so logout brings back the login screen
                var syncDatabaseOnLogin = true;
                while (true)
                {
                    Session.CurrentUser = null;
                    using var loginForm = new LoginForm(syncDatabaseOnLoad: syncDatabaseOnLogin);
                    if (loginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        break;

                    syncDatabaseOnLogin = false;

                    using var main = new Main();
                    Application.Run(main);

                    // If the user didn't request logout, exit the app
                    if (!main.LogoutRequested)
                        break;
                }
            }
            catch (Exception ex)
            {
                var error = $"Amiar Store Manager n'a pas pu démarrer :{Environment.NewLine}{Environment.NewLine}{ex}";
                try
                {
                    var logFolder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "AmiarStoreManager");
                    Directory.CreateDirectory(logFolder);
                    File.AppendAllText(Path.Combine(logFolder, "startup.log"), $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}{error}{Environment.NewLine}{Environment.NewLine}");
                }
                catch
                {
                    // ignore logging failures
                }

                MessageBox.Show(error, "Amiar Store Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}