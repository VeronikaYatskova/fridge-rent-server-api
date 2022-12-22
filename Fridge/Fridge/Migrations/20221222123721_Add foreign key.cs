using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fridge.Migrations
{
    public partial class Addforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fridges_OwnerId",
                table: "Fridges",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fridges_Users_OwnerId",
                table: "Fridges",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fridges_Users_OwnerId",
                table: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_OwnerId",
                table: "Fridges");
        }
    }
}
