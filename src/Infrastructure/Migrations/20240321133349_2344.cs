using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _2344 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScriptId",
                table: "Sheets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Script",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Script", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_ScriptId",
                table: "Sheets",
                column: "ScriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sheets_Script_ScriptId",
                table: "Sheets",
                column: "ScriptId",
                principalTable: "Script",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sheets_Script_ScriptId",
                table: "Sheets");

            migrationBuilder.DropTable(
                name: "Script");

            migrationBuilder.DropIndex(
                name: "IX_Sheets_ScriptId",
                table: "Sheets");

            migrationBuilder.DropColumn(
                name: "ScriptId",
                table: "Sheets");
        }
    }
}
