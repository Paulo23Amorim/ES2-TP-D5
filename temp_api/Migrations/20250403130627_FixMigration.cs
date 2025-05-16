using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_es2.Migrations
{
    /// <inheritdoc />
    public partial class FixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtivosFinanceiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UtilizadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Imposto = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtivosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtivosFinanceiros_Utilizadores_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UtilizadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Utilizadores_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositosPrazos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "numeric", nullable: false),
                    Banco = table.Column<string>(type: "text", nullable: false),
                    NumeroConta = table.Column<string>(type: "text", nullable: false),
                    Titulares = table.Column<string>(type: "text", nullable: false),
                    TaxaJuroAnual = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositosPrazos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositosPrazos_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundosInvestimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MontanteInvestido = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxaJuroPadrao = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundosInvestimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundosInvestimentos_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImovelArrendados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Localizacao = table.Column<string>(type: "text", nullable: false),
                    ValorImovel = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorRenda = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorCondominio = table.Column<decimal>(type: "numeric", nullable: true),
                    DespesasAnuais = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImovelArrendados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImovelArrendados_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Impostos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AtivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValorPago = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impostos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impostos_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Juros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FundoId = table.Column<Guid>(type: "uuid", nullable: false),
                    MesReferencia = table.Column<int>(type: "integer", nullable: false),
                    TaxaJuro = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Juros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Juros_FundosInvestimentos_FundoId",
                        column: x => x.FundoId,
                        principalTable: "FundosInvestimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtivosFinanceiros_UtilizadorId",
                table: "AtivosFinanceiros",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositosPrazos_AtivoId",
                table: "DepositosPrazos",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FundosInvestimentos_AtivoId",
                table: "FundosInvestimentos",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImovelArrendados_AtivoId",
                table: "ImovelArrendados",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Impostos_AtivoId",
                table: "Impostos",
                column: "AtivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UtilizadorId",
                table: "Invoices",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Juros_FundoId",
                table: "Juros",
                column: "FundoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositosPrazos");

            migrationBuilder.DropTable(
                name: "ImovelArrendados");

            migrationBuilder.DropTable(
                name: "Impostos");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Juros");

            migrationBuilder.DropTable(
                name: "FundosInvestimentos");

            migrationBuilder.DropTable(
                name: "AtivosFinanceiros");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
