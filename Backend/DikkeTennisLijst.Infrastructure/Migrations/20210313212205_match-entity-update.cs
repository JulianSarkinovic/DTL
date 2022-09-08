using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DikkeTennisLijst.Infrastructure.Migrations
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Editting migration file names is not a great idea.")]
    public partial class matchentityupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "340e215a-5c67-4e5f-a3ec-076f1e85034a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "745b140b-42b0-42bb-a6f3-4acbd1c93f7b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ea53b5f-00b2-4070-a2aa-287f98f8b5d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7b9b0e4-dc49-4feb-ad07-2f86965898a2");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmationToken",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a0045cb1-8a2c-4f9f-815d-86377d8f9ec2", "320ba36c-fab6-449a-be5f-47bf03c185de", "Player", "PLAYER" },
                    { "936368b3-2af0-4bed-a9f9-68b5e1205bce", "ed19e9cf-7835-478d-a5f4-1e6482fd79fb", "Manager", "MANAGER" },
                    { "78f648f7-d138-444f-bf33-e50d3a1f7e8c", "09596787-386b-4251-889c-6210ad14b3c1", "Admin", "ADMIN" },
                    { "41f1ed67-3c14-4b94-8268-66d363a1669b", "5c86b2df-8d9a-4ec3-b313-404a902b6205", "Developer", "DEVELOPER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41f1ed67-3c14-4b94-8268-66d363a1669b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78f648f7-d138-444f-bf33-e50d3a1f7e8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "936368b3-2af0-4bed-a9f9-68b5e1205bce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0045cb1-8a2c-4f9f-815d-86377d8f9ec2");

            migrationBuilder.DropColumn(
                name: "ConfirmationToken",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Matches");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9ea53b5f-00b2-4070-a2aa-287f98f8b5d2", "2fb948d3-d68a-4483-80cf-9a8b28973e73", "Player", "PLAYER" },
                    { "f7b9b0e4-dc49-4feb-ad07-2f86965898a2", "eed181cc-e3c3-4868-aca5-d89e0dc3bd86", "Manager", "MANAGER" },
                    { "745b140b-42b0-42bb-a6f3-4acbd1c93f7b", "162cda98-a615-4c5c-b710-7a0318cc898c", "Admin", "ADMIN" },
                    { "340e215a-5c67-4e5f-a3ec-076f1e85034a", "9bbf64a3-38ca-4d9b-8374-00e22ecaf8a4", "Developer", "DEVELOPER" }
                });
        }
    }
}