using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infBez.Context.Migrations
{
    /// <inheritdoc />
    public partial class addUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AttachedFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFiles_UserId",
                table: "AttachedFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachedFiles_Users_UserId",
                table: "AttachedFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachedFiles_Users_UserId",
                table: "AttachedFiles");

            migrationBuilder.DropIndex(
                name: "IX_AttachedFiles_UserId",
                table: "AttachedFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AttachedFiles");
        }
    }
}
