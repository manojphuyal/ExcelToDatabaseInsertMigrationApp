using Microsoft.EntityFrameworkCore.Migrations;

namespace FileApp.Migrations
{
    public partial class oldbudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldBudget",
                table: "TenderBasics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldBudget",
                table: "TenderBasics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
