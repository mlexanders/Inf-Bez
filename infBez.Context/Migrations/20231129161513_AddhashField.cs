using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infBez.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddhashField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "AttachedFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "AttachedFiles");
        }
    }
}
