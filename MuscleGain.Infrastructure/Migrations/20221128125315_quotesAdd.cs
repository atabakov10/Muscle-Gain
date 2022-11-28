using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class quotesAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_Languages_LanguageId",
                table: "Proteins");

            migrationBuilder.DropIndex(
                name: "IX_Proteins_LanguageId",
                table: "Proteins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Proteins");

            migrationBuilder.RenameTable(
                name: "Languages",
                newName: "Quotes");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Quotes",
                type: "nvarchar(2046)",
                maxLength: 2046,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Quotes",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Quotes");

            migrationBuilder.RenameTable(
                name: "Quotes",
                newName: "Languages");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Proteins",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

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
    }
}
