namespace resim_ekle.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content  { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
