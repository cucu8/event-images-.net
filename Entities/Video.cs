namespace resim_ekle.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public byte[] VideoData { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public int DurationSeconds { get; set; } // Video süresi saniye cinsinden
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}