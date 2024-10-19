using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminServer.API.Migrations
{
    public partial class ApplierAddForeignKey3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_ApplierForAdmin_ApplierId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Applier_ApplierId",
                table: "Results",
                column: "ApplierId",
                principalTable: "Applier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Applier_ApplierId",
                table: "Results");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Results_ApplierForAdmin_ApplierId",
                table: "Results",
                column: "ApplierId",
                principalTable: "ApplierForAdmin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
