using static System.Net.Mime.MediaTypeNames;

namespace resim_ekle.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Role { get; set; }
        public string? Name { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public InvitationImage? Invitation { get; set; }
        public string? LocationName { get; set; }    
        public double? Latitude { get; set; }     
        public double? Longitude { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
