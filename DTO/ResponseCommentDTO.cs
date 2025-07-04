namespace resim_ekle.DTO
{
    public class ResponseCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
