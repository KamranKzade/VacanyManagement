using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminServer.API.Migrations
{
    public partial class SendAnswerResponeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplierTestResponse",
                table: "Applier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplierTestResponse",
                table: "Applier");
        }
    }
}
