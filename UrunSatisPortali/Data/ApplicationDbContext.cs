
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models;

namespace UrunSatisPortali.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public DbSet<OrderHeader> OrderHeaders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Çanta" },
                new Category { Id = 2, Name = "Cüzdan" },
                new Category { Id = 3, Name = "Aksesuar" }
            );

            // Seed Products
            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Bez Çanta - Soyut Turuncu", Description = "Modern ve soyut desenli bez çanta.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\1.jpg" },
                new Product { Id = 2, Name = "Bez Çanta - Limonlu", Description = "Canlı limon desenli yazlık çanta.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\2.jpg" },
                new Product { Id = 3, Name = "Bez Çanta - Yeşil Eller", Description = "Sanatsal el figürlü tasarım.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\3.jpg" },
                new Product { Id = 4, Name = "Bez Çanta - Mavi Dalgali", Description = "Mavi dalgalı desenli şık çanta.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\4.jpg" },
                new Product { Id = 5, Name = "Bez Çanta - İllüstrasyon", Description = "Siyah beyaz illüstrasyon baskılı.", Price = 180, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\5.jpg" },
                new Product { Id = 6, Name = "Siyah Çanta - Minimal", Description = "Sade siyah üzerinde beyaz detaylar.", Price = 200, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\6.jpg" },
                new Product { Id = 7, Name = "Pastel Çanta - Pembe", Description = "Yumuşak pastel tonlarda tasarım.", Price = 160, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\7.jpg" },
                new Product { Id = 8, Name = "Sarı Çanta - Muzlu", Description = "Eğlenceli muz desenli çanta.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\8.jpg" },
                new Product { Id = 9, Name = "Çizgili Çanta", Description = "Klasik dikey çizgili tasarım.", Price = 140, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\9.jpg" },
                new Product { Id = 10, Name = "Geometrik Çanta", Description = "Geometrik şekillerle modern stil.", Price = 170, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\10.jpg" },
                new Product { Id = 11, Name = "Deri Cüzdan - Kahve", Description = "Hakiki deri kahverengi cüzdan.", Price = 450, Stock = 50, CategoryId = 2, ImagePath = "\\images\\products\\11.jpg" },
                new Product { Id = 12, Name = "Deri Cüzdan - Siyah", Description = "Klasik siyah deri cüzdan.", Price = 450, Stock = 50, CategoryId = 2, ImagePath = "\\images\\products\\12.jpg" },
                new Product { Id = 13, Name = "Kartlık - Mavi", Description = "Minimalist mavi kartlık.", Price = 120, Stock = 50, CategoryId = 2, ImagePath = "\\images\\products\\13.jpg" },
                new Product { Id = 14, Name = "Anahtarlık - Örgü", Description = "El yapımı örgü anahtarlık.", Price = 50, Stock = 50, CategoryId = 3, ImagePath = "\\images\\products\\14.jpg" },
                new Product { Id = 15, Name = "Şapka - Balıkçı", Description = "Trend balıkçı şapkası.", Price = 220, Stock = 50, CategoryId = 3, ImagePath = "\\images\\products\\15.jpg" },
                new Product { Id = 16, Name = "Bez Çanta - Kaktüs", Description = "Sevimli kaktüs desenleri.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\16.jpg" },
                new Product { Id = 17, Name = "Bez Çanta - Uzay", Description = "Gezegen ve yıldız temalı.", Price = 160, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\17.jpg" },
                new Product { Id = 18, Name = "Bez Çanta - Kedili", Description = "Kedi severler için özel tasarım.", Price = 155, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\18.jpg" },
                new Product { Id = 19, Name = "Bez Çanta - Çiçekli", Description = "Bahar esintili çiçek desenli.", Price = 150, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\19.jpg" },
                new Product { Id = 20, Name = "Bez Çanta - Tipografi", Description = "Motivasyon sözleri içeren baskı.", Price = 145, Stock = 50, CategoryId = 1, ImagePath = "\\images\\products\\20.jpg" }
            );
        }
    }
}
