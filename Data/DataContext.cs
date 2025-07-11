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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<InvitationImage> InvitationImages { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvitationImage>()
                .ToTable("InvitationImages")
                .HasOne(ii => ii.User)
                .WithOne(u => u.Invitation)
                .HasForeignKey<InvitationImage>(ii => ii.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Video>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

