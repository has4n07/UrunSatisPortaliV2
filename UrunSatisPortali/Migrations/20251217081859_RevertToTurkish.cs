using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrunSatisPortali.Migrations
{
    /// <inheritdoc />
    public partial class RevertToTurkish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "Stok");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ResimYolu");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "Ad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stok",
                table: "Products",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "ResimYolu",
                table: "Products",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "OrderDetails",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Categories",
                newName: "Name");
        }
    }
}
