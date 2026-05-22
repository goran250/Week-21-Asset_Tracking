using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerAssets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    modelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    purchasePriceUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    localPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    serialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    warrantyExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerAssets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MobileAssets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    modelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    purchasePriceUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    localPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    serialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    warrantyExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileAssets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeAssets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    modelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    purchasePriceUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    localPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    serialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    warrantyExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeAssets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    currencyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerAssets");

            migrationBuilder.DropTable(
                name: "MobileAssets");

            migrationBuilder.DropTable(
                name: "OfficeAssets");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
