using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DikkeTennisLijst.Infrastructure.Migrations
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Editting migration file names is not a great idea.")]
    public partial class tobesure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20430a82-2eaf-4dd1-9862-669a2e6a8563");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46abe379-4140-4709-b0fd-91953a2acc58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa9f9f4d-42cd-4347-9096-a66b641b03d6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5778aca-b7b1-497d-b259-aa0a71de6670");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "97f62906-e2c7-4f4d-b290-52687d07727b", "d7282778-442e-4129-8bb4-2564f690089d", "Player", "PLAYER" },
                    { "62e0186d-58f2-43b7-ac2c-a9520ef0b756", "494a4c7c-f7c4-4c06-a053-a52d1f85760b", "Manager", "MANAGER" },
                    { "b869e66c-cc93-4990-9bd9-cf74e381ef9e", "2ef91869-18ea-4731-9828-7fc826e799c3", "Admin", "ADMIN" },
                    { "f7f37baf-4d07-42d1-8bd4-8be4e2b52432", "a887778d-492a-4a2c-b968-8b936ce0be79", "Developer", "DEVELOPER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62e0186d-58f2-43b7-ac2c-a9520ef0b756");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97f62906-e2c7-4f4d-b290-52687d07727b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b869e66c-cc93-4990-9bd9-cf74e381ef9e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7f37baf-4d07-42d1-8bd4-8be4e2b52432");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20430a82-2eaf-4dd1-9862-669a2e6a8563", "1c4f9e78-b2da-4224-a3e8-85ba09fe9f83", "Player", "PLAYER" },
                    { "46abe379-4140-4709-b0fd-91953a2acc58", "7356826c-2c64-46e2-8f46-08c5c2da39ba", "Manager", "MANAGER" },
                    { "b5778aca-b7b1-497d-b259-aa0a71de6670", "d0edba82-7c15-4468-949a-567edaee4346", "Admin", "ADMIN" },
                    { "aa9f9f4d-42cd-4347-9096-a66b641b03d6", "8e0a9312-3961-4ab6-9093-880effe5a3dc", "Developer", "DEVELOPER" }
                });
        }
    }
}