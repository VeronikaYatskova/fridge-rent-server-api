using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fridge.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FridgeProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fridges",
                columns: table => new
                {
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRented = table.Column<bool>(type: "bit", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridges", x => x.FridgeId);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Producer",
                columns: table => new
                {
                    ProducerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProducerName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ProducerCountry = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producer", x => x.ProducerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductPictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DefaultQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "RentDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFridges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FridgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "Id", "ModelName" },
                values: new object[,]
                {
                    { new Guid("2182354c-d8cc-47bf-844f-4aafaba1dbfe"), "ATLANT ХМ 4625-101 NL" },
                    { new Guid("44dc042a-3453-4c17-a4d1-cd8c0ac9378c"), "ATLANT XM-4208-000" },
                    { new Guid("4a645006-5621-4536-9490-e1769fac2f53"), "LG GA-B379SLUL" },
                    { new Guid("af96137e-0b17-41b5-a819-a5a23da0fd97"), "Toshiba GR-RF610WE-PMS" },
                    { new Guid("f8a3b786-d4b2-49d7-953b-578729b55a35"), "Indesit ITR 5200 W" }
                });

            migrationBuilder.InsertData(
                table: "Producer",
                columns: new[] { "ProducerId", "ProducerCountry", "ProducerName" },
                values: new object[,]
                {
                    { new Guid("0d08c561-361c-497e-bd21-06a7ce7d5516"), "China", "Toshiba" },
                    { new Guid("38886c70-4593-47ce-9cd1-99d9831c2eb4"), "Russia", "LG" },
                    { new Guid("8e652090-8fa2-4271-8e05-7934a0ba77a7"), "Russia", "BEKO" },
                    { new Guid("a8000178-a46b-4122-8758-2931e99c46e9"), "Russia", "Indesit" },
                    { new Guid("d347dfe3-5cf9-49e8-8137-8880580f203b"), "Belarus", "ATLANT" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "DefaultQuantity", "ProductName" },
                values: new object[,]
                {
                    { new Guid("36b74198-a896-429f-b040-0512fca189a8"), 3, "Apple" },
                    { new Guid("3ab533b7-2ae5-4121-85dd-9d977e1b53ed"), 5, "Tomato" },
                    { new Guid("b89aa809-9fac-4c67-af3b-6599ade45f92"), 1, "Cake" },
                    { new Guid("f2ddea9c-7691-4c7a-99ec-abaec36db9bd"), 1, "Milk" },
                    { new Guid("fdb08eb6-d113-4d8a-8576-3454bb89ad55"), 10, "Eggs" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFridges_RentDocumentId",
                table: "UserFridges",
                column: "RentDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FridgeProducts");

            migrationBuilder.DropTable(
                name: "Fridges");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Producer");

            migrationBuilder.DropTable(
                name: "ProductPictures");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UserFridges");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RentDocuments");
        }
    }
}
