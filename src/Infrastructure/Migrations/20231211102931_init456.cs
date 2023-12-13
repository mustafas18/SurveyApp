using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init456 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputValue",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionAnswerId",
                table: "UserAnswers");

            migrationBuilder.RenameColumn(
                name: "UserSurveyId",
                table: "UserAnswers",
                newName: "SurveyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAnswers",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "InputValues",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputValues",
                table: "UserAnswers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserAnswers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "UserAnswers",
                newName: "UserSurveyId");

            migrationBuilder.AddColumn<string>(
                name: "InputValue",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionAnswerId",
                table: "UserAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
