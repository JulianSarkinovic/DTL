using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace DikkeTennisLijst.Infrastructure.Migrations
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Editting migration file names is not a great idea.")]
    public partial class dbsetplayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Matches_MatchId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_EloCasual_History_Matches_MatchId",
                table: "EloCasual_History");

            migrationBuilder.DropForeignKey(
                name: "FK_EloRanked_History_Matches_MatchId",
                table: "EloRanked_History");

            migrationBuilder.DropIndex(
                name: "IX_EloRanked_History_MatchId",
                table: "EloRanked_History");

            migrationBuilder.DropIndex(
                name: "IX_EloCasual_History_MatchId",
                table: "EloCasual_History");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MatchId",
                table: "Comments");

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
                    { "fd67ebd7-b26b-4c10-b186-0878643ed981", "c22877b1-9eb8-44a0-a295-7ca74d46505f", "Player", "PLAYER" },
                    { "b2aeb928-8c20-474c-bca8-cb02d7148985", "18003f20-7c60-467a-b076-7796a199aa00", "Manager", "MANAGER" },
                    { "daa478c1-a67b-4a78-9762-78ccc1689503", "4fa514de-4d0c-43f6-8a1f-6a7e543e4c55", "Admin", "ADMIN" },
                    { "728dd2c2-32d9-441a-9a8e-eb09d3744625", "23fef1d2-87bb-4946-be3e-7b2f6e83bacb", "Developer", "DEVELOPER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_EloRanked_History_MatchId",
                table: "EloRanked_History",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_EloCasual_History_MatchId",
                table: "EloCasual_History",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MatchId",
                table: "Comments",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Matches_MatchId",
                table: "Comments",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EloCasual_History_Matches_MatchId",
                table: "EloCasual_History",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EloRanked_History_Matches_MatchId",
                table: "EloRanked_History",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}