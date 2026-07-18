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

            // Ensure the SQLite database and all tables are created on first run
            using (var db = new KabyliaTaste.Data.AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            Application.Run(new Main());
        }
    }
}