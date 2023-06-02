﻿// <auto-generated />
using System;
using GESTRF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GESTRF.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GESTRF.Models.Chamado", b =>
                {
                    b.Property<int>("ChamadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Assunto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataCri")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Doc")
                        .HasColumnType("longtext");

                    b.Property<string>("Setor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tecnico")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ChamadoId");

                    b.ToTable("Chamado", (string)null);
                });

            modelBuilder.Entity("GESTRF.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Perfil")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("GESTRF.Models.Mensagem", b =>
            {
                b.Property<int>("MensagemId")
                       .ValueGeneratedOnAdd()
                       .HasColumnType("int");

                b.Property<string>("ChamadoFk")
                   .HasColumnType("longtext");

                b.Property<string>("Msg")
                    .HasColumnType("longtext");

                b.HasKey("MensagemId");

                b.ToTable("Mensagem", (string)null);
            });



#pragma warning restore 612, 618
        }
    }
}
