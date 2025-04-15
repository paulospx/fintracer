using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracer.Migrations
{
    /// <inheritdoc />
    public partial class update10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comparisons",
                table: "Comparisons");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Comparisons");

            migrationBuilder.DropColumn(
                name: "CacheRmse",
                table: "Comparisons");

            migrationBuilder.RenameColumn(
                name: "Mad",
                table: "Comparisons",
                newName: "Delta");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Comparisons",
                newName: "TargetFile");

            migrationBuilder.RenameColumn(
                name: "GenerateBy",
                table: "Comparisons",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Comparisons",
                newName: "TargetCurve");

            migrationBuilder.RenameColumn(
                name: "CachedResult",
                table: "Comparisons",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "ComparisonId",
                table: "Comparisons",
                newName: "SourceFile");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comparisons",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Comparisons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Comparisons",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SourceCurve",
                table: "Comparisons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comparisons",
                table: "Comparisons",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comparisons",
                table: "Comparisons");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Comparisons");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Comparisons");

            migrationBuilder.DropColumn(
                name: "SourceCurve",
                table: "Comparisons");

            migrationBuilder.RenameColumn(
                name: "TargetFile",
                table: "Comparisons",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "TargetCurve",
                table: "Comparisons",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "SourceFile",
                table: "Comparisons",
                newName: "ComparisonId");

            migrationBuilder.RenameColumn(
                name: "Delta",
                table: "Comparisons",
                newName: "Mad");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Comparisons",
                newName: "GenerateBy");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Comparisons",
                newName: "CachedResult");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comparisons",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Comparisons",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CacheRmse",
                table: "Comparisons",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comparisons",
                table: "Comparisons",
                column: "ComparisonId");
        }
    }
}
