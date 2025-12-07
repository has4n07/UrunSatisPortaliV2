using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrunSatisPortali.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciModeliGuncellendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilResmiUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilResmiUrl",
                table: "AspNetUsers");
        }
    }
}
