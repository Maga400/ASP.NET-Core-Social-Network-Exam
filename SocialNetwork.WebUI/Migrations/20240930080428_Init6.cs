using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class Init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SenderId",
                table: "Comments",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_SenderId",
                table: "Comments",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_SenderId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SenderId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Comments");
        }
    }
}
