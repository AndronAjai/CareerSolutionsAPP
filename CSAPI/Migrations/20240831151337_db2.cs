using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSAPI.Migrations
{
    public partial class db2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Applications_ApplicationID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ApplicationID",
                table: "Notifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationID",
                table: "Notifications",
                column: "ApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Applications_ApplicationID",
                table: "Notifications",
                column: "ApplicationID",
                principalTable: "Applications",
                principalColumn: "ApplicationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
