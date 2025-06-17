namespace resim_ekle.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}