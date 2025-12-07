using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models; // Proje adına göre namespace'i düzelt

namespace UrunSatisPortali.Data // Proje adına göre namespace'i düzelt
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
            
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<UrunResmi> UrunResimleri { get; set; }


        // Fiyat (decimal) uyarısını düzeltmek için
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Urun>()
                .Property(u => u.Fiyat)
                .HasColumnType("decimal(18, 2)");
        }
    }
}