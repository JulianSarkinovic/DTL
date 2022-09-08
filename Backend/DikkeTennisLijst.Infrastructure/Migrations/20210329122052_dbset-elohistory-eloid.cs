using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DikkeTennisLijst.Infrastructure.Migrations
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Editting migration file names is not a great idea.")]
    public partial class dbsetelohistoryeloid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "728dd2c2-32d9-441a-9a8e-eb09d3744625");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2aeb928-8c20-474c-bca8-cb02d7148985");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "daa478c1-a67b-4a78-9762-78ccc1689503");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd67ebd7-b26b-4c10-b186-0878643ed981");

            migrationBuilder.RenameColumn(
                name: "Elo",
                table: "EloRanked_History",
                newName: "EloId");

            migrationBuilder.RenameColumn(
                name: "Elo",
                table: "EloCasual_History",
                newName: "EloId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "adc5bf64-b42d-493c-8248-96282e0ea624", "3da5b3f5-36a5-4d05-9672-1e204a671af0", "Player", "PLAYER" },
                    { "46f29651-410c-4b63-b339-44d908ea1eca", "1e2ff560-8123-45a7-a161-e1fb5f310811", "Manager", "MANAGER" },
                    { "469498ad-5ab4-4688-bd25-cd39e305d8b0", "2cd6312c-04c9-4cc2-9e0f-86d3a97db52b", "Admin", "ADMIN" },
                    { "eafd8af8-5eb4-46f2-b6f4-ab28d1c073ce", "0fc7f376-c320-4513-a9ed-46e2488a5e44", "Developer", "DEVELOPER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "469498ad-5ab4-4688-bd25-cd39e305d8b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46f29651-410c-4b63-b339-44d908ea1eca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adc5bf64-b42d-493c-8248-96282e0ea624");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eafd8af8-5eb4-46f2-b6f4-ab28d1c073ce");

            migrationBuilder.RenameColumn(
                name: "EloId",
                table: "EloRanked_History",
                newName: "Elo");

            migrationBuilder.RenameColumn(
                name: "EloId",
                table: "EloCasual_History",
                newName: "Elo");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "fd67ebd7-b26b-4c10-b186-0878643ed981", "c22877b1-9eb8-44a0-a295-7ca74d46505f", "Player", "PLAYER" },
                    { "b2aeb928-8c20-474c-bca8-cb02d7148985", "18003f20-7c60-467a-b076-7796a199aa00", "Manager", "MANAGER" },
                    { "daa478c1-a67b-4a78-9762-78ccc1689503", "4fa514de-4d0c-43f6-8a1f-6a7e543e4c55", "Admin", "ADMIN" },
                    { "728dd2c2-32d9-441a-9a8e-eb09d3744625", "23fef1d2-87bb-4946-be3e-7b2f6e83bacb", "Developer", "DEVELOPER" }
                });
        }
    }
}