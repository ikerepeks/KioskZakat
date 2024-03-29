﻿// <auto-generated />
using KioskZakat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KioskZakat.Migrations
{
    [DbContext(typeof(KioskZakatContext))]
    [Migration("20210329043019_initial-setup")]
    partial class initialsetup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KioskZakat.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("imgPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("itemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("price")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("KioskZakat.Models.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("cardNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("itemID")
                        .HasColumnType("int");

                    b.Property<string>("price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("transacTrace")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("itemID");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("KioskZakat.Models.Purchase", b =>
                {
                    b.HasOne("KioskZakat.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("itemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
