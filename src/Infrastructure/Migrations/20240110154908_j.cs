using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class j : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_UserInfos_UserInfoId",
                table: "UserCategories");

            migrationBuilder.DropIndex(
                name: "IX_UserCategories_UserInfoId",
                table: "UserCategories");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "UserCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UserInfos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_CategoryId",
                table: "UserInfos",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId",
                table: "UserInfos",
                column: "CategoryId",
                principalTable: "UserCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId",
                table: "UserInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserInfos_CategoryId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UserInfos");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "UserCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCategories_UserInfoId",
                table: "UserCategories",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_UserInfos_UserInfoId",
                table: "UserCategories",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }
    }
}
