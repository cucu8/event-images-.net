using static System.Net.Mime.MediaTypeNames;

namespace resim_ekle.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Role { get; set; }
        public string? Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public InvitationImage? Invitation { get; set; }
        public string? LocationName { get; set; }    
        public double? Latitude { get; set; }     
        public double? Longitude { get; set; }
        public DateTime? InvitedAt { get; set; }
    }
}
