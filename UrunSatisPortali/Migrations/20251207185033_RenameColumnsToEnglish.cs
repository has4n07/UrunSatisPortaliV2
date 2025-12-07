using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrunSatisPortali.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnsToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriId",
                table: "Urunler");

            migrationBuilder.RenameColumn(
                name: "Stok",
                table: "Urunler",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "ResimYolu",
                table: "Urunler",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "KategoriId",
                table: "Urunler",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Urunler",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Urunler",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Urunler",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_Urunler_KategoriId",
                table: "Urunler",
                newName: "IX_Urunler_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Kategoriler",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Kategoriler_CategoryId",
                table: "Urunler",
                column: "CategoryId",
                principalTable: "Kategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Kategoriler_CategoryId",
                table: "Urunler");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Urunler",
                newName: "Stok");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Urunler",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Urunler",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Urunler",
                newName: "ResimYolu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Urunler",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Urunler",
                newName: "KategoriId");

            migrationBuilder.RenameIndex(
                name: "IX_Urunler_CategoryId",
                table: "Urunler",
                newName: "IX_Urunler_KategoriId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Kategoriler",
                newName: "Ad");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriId",
                table: "Urunler",
                column: "KategoriId",
                principalTable: "Kategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
