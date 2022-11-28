using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class quotesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Quotes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Quotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_UserId",
                table: "Quotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_UserId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Quotes");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Quotes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
