using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_es2.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirUtilizador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Utilizadores");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Utilizadores",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Utilizadores",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Utilizadores",
                newName: "TipoUtilizador");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Utilizadores",
                newName: "user_id");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "Utilizadores",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Utilizadores",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Utilizadores",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Utilizadores");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Utilizadores",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Utilizadores",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "TipoUtilizador",
                table: "Utilizadores",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Utilizadores",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Utilizadores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Utilizadores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Utilizadores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
