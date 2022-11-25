using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class shoppingCartProteinId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proteins_ShoppingCarts_ShoppingCartId",
                table: "Proteins");

            migrationBuilder.DropIndex(
                name: "IX_Proteins_ShoppingCartId",
                table: "Proteins");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Proteins");

            migrationBuilder.CreateTable(
                name: "ProteinShoppingCart",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartProteinsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProteinShoppingCart", x => new { x.ShoppingCartId, x.ShoppingCartProteinsId });
                    table.ForeignKey(
                        name: "FK_ProteinShoppingCart_Proteins_ShoppingCartProteinsId",
                        column: x => x.ShoppingCartProteinsId,
                        principalTable: "Proteins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProteinShoppingCart_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProteinShoppingCart_ShoppingCartProteinsId",
                table: "ProteinShoppingCart",
                column: "ShoppingCartProteinsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProteinShoppingCart");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "Proteins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proteins_ShoppingCartId",
                table: "Proteins",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proteins_ShoppingCarts_ShoppingCartId",
                table: "Proteins",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id");
        }
    }
}
