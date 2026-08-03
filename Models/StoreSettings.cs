namespace KabyliaTaste.Models
{
    public class StoreSettings
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = "KabyliaTaste";
        public byte[]? LogoData { get; set; }
        public string? GoogleDriveClientId { get; set; }
        public string? GoogleDriveClientSecret { get; set; }
        public string? GoogleDriveFolderId { get; set; }
        public string? GoogleDriveRefreshToken { get; set; }
    }
}
