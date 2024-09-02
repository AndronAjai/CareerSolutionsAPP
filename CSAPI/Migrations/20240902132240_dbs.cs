using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSAPI.Migrations
{
    public partial class dbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobStatusNotifications",
                columns: table => new
                {
                    SnID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatusNotifications", x => x.SnID);
                    table.ForeignKey(
                        name: "FK_JobStatusNotifications_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobStatusNotifications_ApplicationID",
                table: "JobStatusNotifications",
                column: "ApplicationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobStatusNotifications");
        }
    }
}
