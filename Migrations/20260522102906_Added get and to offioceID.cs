using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset_Tracking.Migrations
{
    /// <inheritdoc />
    public partial class AddedgetandtooffioceID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "officeID",
                table: "OfficeAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "officeID",
                table: "MobileAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "officeID",
                table: "ComputerAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "officeID",
                table: "OfficeAssets");

            migrationBuilder.DropColumn(
                name: "officeID",
                table: "MobileAssets");

            migrationBuilder.DropColumn(
                name: "officeID",
                table: "ComputerAssets");
        }
    }
}
