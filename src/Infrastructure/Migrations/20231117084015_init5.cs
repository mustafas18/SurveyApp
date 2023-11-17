using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SheetId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SheetId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "SheetUserInfo",
                columns: table => new
                {
                    SheetsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheetUserInfo", x => new { x.SheetsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SheetUserInfo_Sheets_SheetsId",
                        column: x => x.SheetsId,
                        principalTable: "Sheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SheetUserInfo_UserInfos_UsersId",
                        column: x => x.UsersId,
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SheetUserInfo_UsersId",
                table: "SheetUserInfo",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SheetUserInfo");

            migrationBuilder.AddColumn<int>(
                name: "SheetId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SheetId",
                table: "AspNetUsers",
                column: "SheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetId",
                table: "AspNetUsers",
                column: "SheetId",
                principalTable: "Sheets",
                principalColumn: "Id");
        }
    }
}
