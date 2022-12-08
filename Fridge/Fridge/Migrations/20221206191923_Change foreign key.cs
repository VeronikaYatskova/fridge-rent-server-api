using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fridge.Migrations
{
    public partial class Changeforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentDocuments_Fridges_Id",
                table: "RentDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_RentDocuments_FridgeId",
                table: "RentDocuments",
                column: "FridgeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RentDocuments_Fridges_FridgeId",
                table: "RentDocuments",
                column: "FridgeId",
                principalTable: "Fridges",
                principalColumn: "FridgeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentDocuments_Fridges_FridgeId",
                table: "RentDocuments");

            migrationBuilder.DropIndex(
                name: "IX_RentDocuments_FridgeId",
                table: "RentDocuments");

            migrationBuilder.AddForeignKey(
                name: "FK_RentDocuments_Fridges_Id",
                table: "RentDocuments",
                column: "Id",
                principalTable: "Fridges",
                principalColumn: "FridgeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
