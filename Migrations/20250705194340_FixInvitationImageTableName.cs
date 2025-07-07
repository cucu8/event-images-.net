using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resim_ekle.Migrations
{
    /// <inheritdoc />
    public partial class FixInvitationImageTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitationImage_Users_UserId",
                table: "InvitationImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvitationImage",
                table: "InvitationImage");

            migrationBuilder.RenameTable(
                name: "InvitationImage",
                newName: "InvitationImages");

            migrationBuilder.RenameIndex(
                name: "IX_InvitationImage_UserId",
                table: "InvitationImages",
                newName: "IX_InvitationImages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvitationImages",
                table: "InvitationImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationImages_Users_UserId",
                table: "InvitationImages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvitationImages_Users_UserId",
                table: "InvitationImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvitationImages",
                table: "InvitationImages");

            migrationBuilder.RenameTable(
                name: "InvitationImages",
                newName: "InvitationImage");

            migrationBuilder.RenameIndex(
                name: "IX_InvitationImages_UserId",
                table: "InvitationImage",
                newName: "IX_InvitationImage_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvitationImage",
                table: "InvitationImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationImage_Users_UserId",
                table: "InvitationImage",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
