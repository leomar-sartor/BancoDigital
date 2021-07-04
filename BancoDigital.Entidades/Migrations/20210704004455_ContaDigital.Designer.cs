﻿// <auto-generated />
using System;
using BancoDigital.Entidades.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BancoDigital.Entidades.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210704004455_ContaDigital")]
    partial class ContaDigital
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("BancoDigital.Entidades.ContaDigital", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contas");
                });

            modelBuilder.Entity("BancoDigital.Entidades.Movimento", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("ContaId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime");

                    b.Property<short>("Operacao")
                        .HasColumnType("smallint");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ContaId");

                    b.ToTable("Movimentos");
                });

            modelBuilder.Entity("BancoDigital.Entidades.Movimento", b =>
                {
                    b.HasOne("BancoDigital.Entidades.ContaDigital", "Conta")
                        .WithMany("Movimentacoes")
                        .HasForeignKey("ContaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conta");
                });

            modelBuilder.Entity("BancoDigital.Entidades.ContaDigital", b =>
                {
                    b.Navigation("Movimentacoes");
                });
#pragma warning restore 612, 618
        }
    }
}