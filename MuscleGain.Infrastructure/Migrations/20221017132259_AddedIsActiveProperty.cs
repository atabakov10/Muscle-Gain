using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class AddedIsActiveProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Proteins",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Product is active");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Proteins");
        }
    }
}
