using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class langAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Proteins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proteins_LanguageId",
                table: "Proteins",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proteins_Languages_LanguageId",
                table: "Proteins",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_Languages_LanguageId",
                table: "Proteins");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Proteins_LanguageId",
                table: "Proteins");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Proteins");
        }
    }
}
