using Microsoft.EntityFrameworkCore.Migrations;

namespace Churchgoers.Web.Migrations
{
    public partial class DeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Fields_FieldId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name",
                table: "Churches");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts",
                columns: new[] { "Name", "FieldId" },
                unique: true,
                filter: "[FieldId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches",
                columns: new[] { "Name", "DistrictId" },
                unique: true,
                filter: "[DistrictId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Fields_FieldId",
                table: "Districts",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Fields_FieldId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name",
                table: "Districts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name",
                table: "Churches",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Fields_FieldId",
                table: "Districts",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
