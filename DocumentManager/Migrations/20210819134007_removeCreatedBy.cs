using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentManager.Migrations
{
    public partial class removeCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Document");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Document",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
