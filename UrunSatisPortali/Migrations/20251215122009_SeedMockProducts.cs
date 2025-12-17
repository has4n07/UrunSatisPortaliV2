using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrunSatisPortali.Migrations
{
    /// <inheritdoc />
    public partial class SeedMockProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Çanta" },
                    { 2, "Cüzdan" },
                    { 3, "Aksesuar" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "CategoryId", "Price", "ImageUrl", "Stock" },
                values: new object[,]
                {
                    { 1, "Modern ve soyut desenli bez çanta.", "Bez Çanta - Soyut Turuncu", 1, 150m, "\\images\\products\\1.jpg", 50 },
                    { 2, "Canlı limon desenli yazlık çanta.", "Bez Çanta - Limonlu", 1, 150m, "\\images\\products\\2.jpg", 50 },
                    { 3, "Sanatsal el figürlü tasarım.", "Bez Çanta - Yeşil Eller", 1, 150m, "\\images\\products\\3.jpg", 50 },
                    { 4, "Mavi dalgalı desenli şık çanta.", "Bez Çanta - Mavi Dalgali", 1, 150m, "\\images\\products\\4.jpg", 50 },
                    { 5, "Siyah beyaz illüstrasyon baskılı.", "Bez Çanta - İllüstrasyon", 1, 180m, "\\images\\products\\5.jpg", 50 },
                    { 6, "Sade siyah üzerinde beyaz detaylar.", "Siyah Çanta - Minimal", 1, 200m, "\\images\\products\\6.jpg", 50 },
                    { 7, "Yumuşak pastel tonlarda tasarım.", "Pastel Çanta - Pembe", 1, 160m, "\\images\\products\\7.jpg", 50 },
                    { 8, "Eğlenceli muz desenli çanta.", "Sarı Çanta - Muzlu", 1, 150m, "\\images\\products\\8.jpg", 50 },
                    { 9, "Klasik dikey çizgili tasarım.", "Çizgili Çanta", 1, 140m, "\\images\\products\\9.jpg", 50 },
                    { 10, "Geometrik şekillerle modern stil.", "Geometrik Çanta", 1, 170m, "\\images\\products\\10.jpg", 50 },
                    { 11, "Hakiki deri kahverengi cüzdan.", "Deri Cüzdan - Kahve", 2, 450m, "\\images\\products\\11.jpg", 50 },
                    { 12, "Klasik siyah deri cüzdan.", "Deri Cüzdan - Siyah", 2, 450m, "\\images\\products\\12.jpg", 50 },
                    { 13, "Minimalist mavi kartlık.", "Kartlık - Mavi", 2, 120m, "\\images\\products\\13.jpg", 50 },
                    { 14, "El yapımı örgü anahtarlık.", "Anahtarlık - Örgü", 3, 50m, "\\images\\products\\14.jpg", 50 },
                    { 15, "Trend balıkçı şapkası.", "Şapka - Balıkçı", 3, 220m, "\\images\\products\\15.jpg", 50 },
                    { 16, "Sevimli kaktüs desenleri.", "Bez Çanta - Kaktüs", 1, 150m, "\\images\\products\\16.jpg", 50 },
                    { 17, "Gezegen ve yıldız temalı.", "Bez Çanta - Uzay", 1, 160m, "\\images\\products\\17.jpg", 50 },
                    { 18, "Kedi severler için özel tasarım.", "Bez Çanta - Kedili", 1, 155m, "\\images\\products\\18.jpg", 50 },
                    { 19, "Bahar esintili çiçek desenli.", "Bez Çanta - Çiçekli", 1, 150m, "\\images\\products\\19.jpg", 50 },
                    { 20, "Motivasyon sözleri içeren baskı.", "Bez Çanta - Tipografi", 1, 145m, "\\images\\products\\20.jpg", 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
