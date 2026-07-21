namespace KabyliaTaste.Models
{
    public class StoreSettings
    {
        public int Id { get; set; }
        public string StoreName { get; set; } = "KabyliaTaste";
        public byte[]? LogoData { get; set; }
    }
}
