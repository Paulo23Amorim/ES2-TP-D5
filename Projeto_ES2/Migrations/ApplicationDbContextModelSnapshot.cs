﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Projeto_ES2.Components.Data;

#nullable disable

namespace Projeto_es2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Projeto_ES2.Components.Models.AtivoFinanceiro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Imposto")
                        .HasColumnType("numeric");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Tipo")
                        .HasColumnType("integer");

                    b.Property<Guid>("UtilizadorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UtilizadorId");

                    b.ToTable("AtivosFinanceiros");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.DepositoPrazo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AtivoId")
                        .HasColumnType("uuid");

                    b.Property<string>("Banco")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumeroConta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TaxaJuroAnual")
                        .HasColumnType("numeric");

                    b.Property<string>("Titulares")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ValorInicial")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId")
                        .IsUnique();

                    b.ToTable("DepositosPrazos");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.FundoInvestimento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AtivoId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("MontanteInvestido")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TaxaJuroPadrao")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId")
                        .IsUnique();

                    b.ToTable("FundosInvestimentos");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.ImovelArrendado", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AtivoId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("DespesasAnuais")
                        .HasColumnType("numeric");

                    b.Property<string>("Localizacao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("ValorCondominio")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ValorImovel")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("ValorRenda")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId")
                        .IsUnique();

                    b.ToTable("ImovelArrendados");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Imposto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AtivoId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DataPagamento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("ValorPago")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId");

                    b.ToTable("Impostos");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Descricao")
                        .HasColumnType("text");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UtilizadorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UtilizadorId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Juros", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FundoId")
                        .HasColumnType("uuid");

                    b.Property<int>("MesReferencia")
                        .HasColumnType("integer");

                    b.Property<decimal>("TaxaJuro")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("FundoId");

                    b.ToTable("Juros");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Utilizador", b =>
                {
                    b.Property<Guid>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("TipoUtilizador")
                        .HasColumnType("integer");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("user_id");

                    b.ToTable("Utilizadores");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.AtivoFinanceiro", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.Utilizador", "Utilizador")
                        .WithMany("AtivosFinanceiros")
                        .HasForeignKey("UtilizadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.DepositoPrazo", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.AtivoFinanceiro", "AtivoFinanceiro")
                        .WithOne("DepositoPrazo")
                        .HasForeignKey("Projeto_ES2.Components.Models.DepositoPrazo", "AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AtivoFinanceiro");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.FundoInvestimento", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.AtivoFinanceiro", "AtivoFinanceiro")
                        .WithOne("FundoInvestimento")
                        .HasForeignKey("Projeto_ES2.Components.Models.FundoInvestimento", "AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AtivoFinanceiro");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.ImovelArrendado", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.AtivoFinanceiro", "AtivoFinanceiro")
                        .WithOne("ImovelArrendado")
                        .HasForeignKey("Projeto_ES2.Components.Models.ImovelArrendado", "AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AtivoFinanceiro");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Imposto", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.AtivoFinanceiro", "AtivoFinanceiro")
                        .WithMany("Impostos")
                        .HasForeignKey("AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AtivoFinanceiro");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Invoice", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.Utilizador", "Utilizador")
                        .WithMany("Invoices")
                        .HasForeignKey("UtilizadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Juros", b =>
                {
                    b.HasOne("Projeto_ES2.Components.Models.FundoInvestimento", "FundoInvestimento")
                        .WithMany("Juros")
                        .HasForeignKey("FundoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FundoInvestimento");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.AtivoFinanceiro", b =>
                {
                    b.Navigation("DepositoPrazo");

                    b.Navigation("FundoInvestimento");

                    b.Navigation("ImovelArrendado");

                    b.Navigation("Impostos");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.FundoInvestimento", b =>
                {
                    b.Navigation("Juros");
                });

            modelBuilder.Entity("Projeto_ES2.Components.Models.Utilizador", b =>
                {
                    b.Navigation("AtivosFinanceiros");

                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
