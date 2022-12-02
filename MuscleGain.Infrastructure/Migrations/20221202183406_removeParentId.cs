using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class removeParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProteinsCategories_ProteinsCategories_ParentId",
                table: "ProteinsCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProteinsCategories_ParentId",
                table: "ProteinsCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProteinsCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ProteinsCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProteinsCategories_ParentId",
                table: "ProteinsCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProteinsCategories_ProteinsCategories_ParentId",
                table: "ProteinsCategories",
                column: "ParentId",
                principalTable: "ProteinsCategories",
                principalColumn: "Id");
        }
    }
}
