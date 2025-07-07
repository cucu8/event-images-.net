namespace resim_ekle.DTO
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
