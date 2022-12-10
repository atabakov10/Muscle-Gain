using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleGain.Infrastructure.Migrations
{
    public partial class usersSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Quotes",
                type: "nvarchar(max)",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2046)",
                oldMaxLength: 2046);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShoppingCartId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "34875379-5ac4-45df-8bd3-7e3bda353ab2", 0, "3b1d6de2-907f-4818-8814-f548453ddb47", "v_tosheva02@abv.bg", false, "Victoria", null, "Tosheva", false, null, "v_tosheva02@abv.bg", "v_tosheva02@abv.bg", "AQAAAAEAACcQAAAAEL1Kv5Hkg/0vBeZkMfGodBrOR0+azkUty/fcMZIjicPpBJLGV8I+bqvQ0OK0Vb/+bA==", null, false, "d8ee80d2-5bd5-4bfe-a360-a507aa6d33f1", null, false, "v_tosheva02@abv.bg" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "ShoppingCartId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e013049a-50da-453a-a95d-1486b1c9f03f", 0, "328387c4-100c-4204-bcb7-87576821ac78", "atabakov99@abv.bg", false, "Angel", "https://img.a.transfermarkt.technology/portrait/big/28003-1631171950.jpg?lm=1", "Tabakov", false, null, "atabakov99@abv.bg", "atabakov99@abv.bg", "AQAAAAEAACcQAAAAEMgHR83HG/0XakPLL65Ts4kvOZxqCspG+y76f2nwFp0nvqKTVWsX8hnCuFDwykNz2g==", null, false, "9b730dc7-c965-475e-a4c3-b2a836462425", null, false, "atabakov99@abv.bg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "34875379-5ac4-45df-8bd3-7e3bda353ab2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e013049a-50da-453a-a95d-1486b1c9f03f");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Quotes",
                type: "nvarchar(2046)",
                maxLength: 2046,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2147483647);
        }
    }
}
