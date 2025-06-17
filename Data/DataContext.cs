using Microsoft.EntityFrameworkCore;
using resim_ekle.Entities;


namespace QrImageUploader.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
            .HasOne(i => i.User)
            .WithMany(u => u.Images)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade); // veya istediğine göre SetNull, Restrict, vb.

            base.OnModelCreating(modelBuilder);
        }
    }
}

