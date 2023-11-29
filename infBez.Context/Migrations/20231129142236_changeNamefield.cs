using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infBez.Context.Migrations
{
    /// <inheritdoc />
    public partial class changeNamefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastAccessTime",
                table: "AttachedFiles",
                newName: "LastWriteTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastWriteTime",
                table: "AttachedFiles",
                newName: "LastAccessTime");
        }
    }
}
