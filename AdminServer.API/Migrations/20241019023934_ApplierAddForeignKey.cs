using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminServer.API.Migrations
{
    public partial class ApplierAddForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Applier",
                table: "Applier");

            migrationBuilder.RenameTable(
                name: "Applier",
                newName: "ApplierForAdmin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplierForAdmin",
                table: "ApplierForAdmin",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ApplierId",
                table: "Results",
                column: "ApplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_VacancyId",
                table: "Results",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ApplierForAdmin_ApplierId",
                table: "Results",
                column: "ApplierId",
                principalTable: "ApplierForAdmin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Vacancies_VacancyId",
                table: "Results",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_ApplierForAdmin_ApplierId",
                table: "Results");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Vacancies_VacancyId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_ApplierId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_VacancyId",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplierForAdmin",
                table: "ApplierForAdmin");

            migrationBuilder.RenameTable(
                name: "ApplierForAdmin",
                newName: "Applier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applier",
                table: "Applier",
                column: "Id");
        }
    }
}
