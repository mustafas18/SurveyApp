using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Messure",
                table: "Variables",
                type: "int",
                nullable: false,
                comment: " 0: Scale,\r\n 1: Nominal,\r\n 2. Ordinal",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "0: Scale,\r\n 1: Nominal,\r\n 2. Ordinal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Messure",
                table: "Variables",
                type: "int",
                nullable: false,
                comment: "0: Scale,\r\n 1: Nominal,\r\n 2. Ordinal",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: " 0: Scale,\r\n 1: Nominal,\r\n 2. Ordinal");
        }
    }
}
