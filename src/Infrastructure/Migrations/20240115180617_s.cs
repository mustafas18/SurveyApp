using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId",
                table: "UserInfos");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "UserInfos",
                newName: "CategoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserInfos_CategoryId",
                table: "UserInfos",
                newName: "IX_UserInfos_CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId1",
                table: "UserInfos",
                column: "CategoryId1",
                principalTable: "UserCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId1",
                table: "UserInfos");

            migrationBuilder.RenameColumn(
                name: "CategoryId1",
                table: "UserInfos",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInfos_CategoryId1",
                table: "UserInfos",
                newName: "IX_UserInfos_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_UserCategories_CategoryId",
                table: "UserInfos",
                column: "CategoryId",
                principalTable: "UserCategories",
                principalColumn: "Id");
        }
    }
}
