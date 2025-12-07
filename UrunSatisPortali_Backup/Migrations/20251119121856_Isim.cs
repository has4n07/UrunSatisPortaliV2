using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrunSatisPortali.Migrations
{
    /// <inheritdoc />
    public partial class Isim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilResmiUrl",
                table: "AspNetUsers",
                newName: "ProfilResmiYolu");

            migrationBuilder.AlterColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilResmiYolu",
                table: "AspNetUsers",
                newName: "ProfilResmiUrl");

            migrationBuilder.AlterColumn<string>(
                name: "AdSoyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
