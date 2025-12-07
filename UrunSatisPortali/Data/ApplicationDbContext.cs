
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<Urun> Urunler { get; set; } = null!;
        public DbSet<Kategori> Kategoriler { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
