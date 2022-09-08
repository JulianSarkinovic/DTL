using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DikkeTennisLijst.Infrastructure.Migrations
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Editting migration file names is not a great idea.")]
    public partial class addconfirmationstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationStatus",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmationStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ConfirmationStatus",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ConfirmationStatus",
                table: "AspNetUsers");

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
    }
}