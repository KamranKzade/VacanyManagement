using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminServer.API.Migrations
{
    public partial class ApplierAddForeignKey2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_VacancyId",
                table: "Results");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                columns: new[] { "VacancyId", "ApplierId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Results_VacancyId",
                table: "Results",
                column: "VacancyId");
        }
    }
}
