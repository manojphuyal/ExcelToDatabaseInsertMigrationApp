using Microsoft.EntityFrameworkCore.Migrations;

namespace FileApp.Migrations
{
    public partial class countryAndCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldBudget",
                table: "TenderBasics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "TenderBasics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "References",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyWords",
                table: "References",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "ReferenceOthers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "ReferenceOthers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldId",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldBudget",
                table: "TenderBasics");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "TenderBasics");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "References");

            migrationBuilder.DropColumn(
                name: "KeyWords",
                table: "References");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "ReferenceOthers");

            migrationBuilder.DropColumn(
                name: "ReferenceCode",
                table: "ReferenceOthers");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");
        }
    }
}
