using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class authorNameQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Quotes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Quotes");
        }
    }
}
