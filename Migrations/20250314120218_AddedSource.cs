using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Timelines",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Timelines");
        }
    }
}
