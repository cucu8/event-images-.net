namespace resim_ekle.Entities
{
    public class InvitationImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
