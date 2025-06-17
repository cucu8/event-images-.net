using static System.Net.Mime.MediaTypeNames;

namespace resim_ekle.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int Role { get; set; }
        public string? Name { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
