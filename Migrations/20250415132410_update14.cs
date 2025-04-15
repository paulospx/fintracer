using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracer.Migrations
{
    /// <inheritdoc />
    public partial class update14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DeltaSum",
                table: "Comparisons",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeltaSum",
                table: "Comparisons");
        }
    }
}
