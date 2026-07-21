namespace KabyliaTaste
{
    using KabyliaTaste.Models;

    public static class Session
    {
        public static User? CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser?.IsAdmin == true;
    }
}
