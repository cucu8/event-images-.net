using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resim_ekle.Migrations
{
    /// <inheritdoc />
    public partial class mg2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
          
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId1",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId1",
                table: "Images");

         
        }
    }
}
