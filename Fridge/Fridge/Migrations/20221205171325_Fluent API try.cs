using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fridge.Migrations
{
    public partial class FluentAPItry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fridges_ModelId",
                table: "Fridges",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fridges_Models_ModelId",
                table: "Fridges",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fridges_Models_ModelId",
                table: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_ModelId",
                table: "Fridges");
        }
    }
}
