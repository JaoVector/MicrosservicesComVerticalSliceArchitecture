﻿// <auto-generated />
using System;
using Game.Inventario.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Game.Inventario.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Game.Inventario.Api.Entities.ItemBatalha", b =>
                {
                    b.Property<Guid>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Ataque")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DataAtualizacao")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("DataCriacao")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DataExclusao")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Defesa")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemCategoria")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.HasKey("ItemId");

                    b.ToTable("ItensBatalha");
                });

            modelBuilder.Entity("Game.Inventario.Api.Entities.ItemInventario", b =>
                {
                    b.Property<Guid>("ItemIventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("DataAtualizacao")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("DataCriacao")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DataExclusao")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonagemId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemIventId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItensInventario");
                });

            modelBuilder.Entity("Game.Inventario.Api.Entities.ItemInventario", b =>
                {
                    b.HasOne("Game.Inventario.Api.Entities.ItemBatalha", "ItemBatalha")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemBatalha");
                });
#pragma warning restore 612, 618
        }
    }
}
