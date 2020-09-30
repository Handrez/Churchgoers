using Microsoft.EntityFrameworkCore.Migrations;

namespace Churchgoers.Web.Migrations
{
    public partial class AddNewEn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistance_Meeting_MeetingId",
                table: "Assistance");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistance_AspNetUsers_UserId",
                table: "Assistance");

            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Churches_ChurchId",
                table: "Meeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assistance",
                table: "Assistance");

            migrationBuilder.RenameTable(
                name: "Meeting",
                newName: "Meetings");

            migrationBuilder.RenameTable(
                name: "Assistance",
                newName: "Assistances");

            migrationBuilder.RenameIndex(
                name: "IX_Meeting_ChurchId",
                table: "Meetings",
                newName: "IX_Meetings_ChurchId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistance_UserId",
                table: "Assistances",
                newName: "IX_Assistances_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistance_MeetingId",
                table: "Assistances",
                newName: "IX_Assistances_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assistances",
                table: "Assistances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assistances",
                table: "Assistances");

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "Meeting");

            migrationBuilder.RenameTable(
                name: "Assistances",
                newName: "Assistance");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_ChurchId",
                table: "Meeting",
                newName: "IX_Meeting_ChurchId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistances_UserId",
                table: "Assistance",
                newName: "IX_Assistance_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistances_MeetingId",
                table: "Assistance",
                newName: "IX_Assistance_MeetingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assistance",
                table: "Assistance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistance_Meeting_MeetingId",
                table: "Assistance",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistance_AspNetUsers_UserId",
                table: "Assistance",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Churches_ChurchId",
                table: "Meeting",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
