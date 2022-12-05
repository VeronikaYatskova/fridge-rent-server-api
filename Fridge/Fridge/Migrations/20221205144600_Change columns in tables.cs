using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fridge.Migrations
{
    public partial class Changecolumnsintables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFridges");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RentDocuments",
                newName: "RenterId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductPictures",
                newName: "RenterId");

            migrationBuilder.CreateTable(
                name: "RenterFridges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenterFridges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RenterFridges_RentDocuments_RentDocumentId",
                        column: x => x.RentDocumentId,
                        principalTable: "RentDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Renters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RenterFridges_RentDocumentId",
                table: "RenterFridges",
                column: "RentDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenterFridges");

            migrationBuilder.DropTable(
                name: "Renters");

            migrationBuilder.RenameColumn(
                name: "RenterId",
                table: "RentDocuments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RenterId",
                table: "ProductPictures",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "UserFridges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFridges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFridges_RentDocuments_RentDocumentId",
                        column: x => x.RentDocumentId,
                        principalTable: "RentDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFridges_RentDocumentId",
                table: "UserFridges",
                column: "RentDocumentId");
        }
    }
}
