using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracer.Migrations
{
    /// <inheritdoc />
    public partial class upodate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filetransfers",
                columns: table => new
                {
                    TransferId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceFile = table.Column<string>(type: "TEXT", nullable: false),
                    SourceCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SourceSize = table.Column<long>(type: "INTEGER", nullable: false),
                    SourceMd5 = table.Column<string>(type: "TEXT", nullable: true),
                    TargetFile = table.Column<string>(type: "TEXT", nullable: false),
                    TargetCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TargetSize = table.Column<long>(type: "INTEGER", nullable: false),
                    TargetMd5 = table.Column<string>(type: "TEXT", nullable: true),
                    LastCopied = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsSelected = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    ScheduledToCopy = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filetransfers", x => x.TransferId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filetransfers");
        }
    }
}
