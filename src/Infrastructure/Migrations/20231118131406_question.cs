using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class question : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Questions",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "QuestionFileUri",
                table: "Questions",
                newName: "FileUri");

            migrationBuilder.RenameColumn(
                name: "QuestionFileContentType",
                table: "Questions",
                newName: "FileContentType");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Questions",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "FileUri",
                table: "Questions",
                newName: "QuestionFileUri");

            migrationBuilder.RenameColumn(
                name: "FileContentType",
                table: "Questions",
                newName: "QuestionFileContentType");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
