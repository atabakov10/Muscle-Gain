using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class updatedCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProteinShoppingCart_Proteins_ShoppingCartCoursesId",
                table: "ProteinShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProteinShoppingCart",
                table: "ProteinShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ProteinShoppingCart_ShoppingCartId",
                table: "ProteinShoppingCart");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartCoursesId",
                table: "ProteinShoppingCart",
                newName: "ShoppingCartProteinsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProteinShoppingCart",
                table: "ProteinShoppingCart",
                columns: new[] { "ShoppingCartId", "ShoppingCartProteinsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProteinShoppingCart_ShoppingCartProteinsId",
                table: "ProteinShoppingCart",
                column: "ShoppingCartProteinsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProteinShoppingCart_Proteins_ShoppingCartProteinsId",
                table: "ProteinShoppingCart",
                column: "ShoppingCartProteinsId",
                principalTable: "Proteins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProteinShoppingCart_Proteins_ShoppingCartProteinsId",
                table: "ProteinShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProteinShoppingCart",
                table: "ProteinShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ProteinShoppingCart_ShoppingCartProteinsId",
                table: "ProteinShoppingCart");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartProteinsId",
                table: "ProteinShoppingCart",
                newName: "ShoppingCartCoursesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProteinShoppingCart",
                table: "ProteinShoppingCart",
                columns: new[] { "ShoppingCartCoursesId", "ShoppingCartId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProteinShoppingCart_ShoppingCartId",
                table: "ProteinShoppingCart",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProteinShoppingCart_Proteins_ShoppingCartCoursesId",
                table: "ProteinShoppingCart",
                column: "ShoppingCartCoursesId",
                principalTable: "Proteins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
