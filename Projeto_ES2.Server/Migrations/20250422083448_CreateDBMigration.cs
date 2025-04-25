using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_es2.Migrations
{
    /// <inheritdoc />
    public partial class CreateDBMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "Utilizadores",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "AtivosFinanceiros",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Utilizadores",
                newName: "password");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "AtivosFinanceiros",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
