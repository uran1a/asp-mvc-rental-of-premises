﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RentalOfPremises.Models;

#nullable disable

namespace RentalOfPremises.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RentalOfPremises.Models.Deal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateOfConclusion")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDateRental")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<int>("PlacementId")
                        .HasColumnType("integer");

                    b.Property<int>("RenterId")
                        .HasColumnType("integer");

                    b.Property<string>("Repairs")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDateRental")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TypeOfActivity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PlacementId")
                        .IsUnique();

                    b.HasIndex("RenterId");

                    b.ToTable("Deal");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("PlacementId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlacementId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("RentalOfPremises.Models.PhysicalEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data_Of_Birth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Mobile_Phone")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Passport_Code")
                        .HasColumnType("integer");

                    b.Property<int>("Passport_Serial")
                        .HasColumnType("integer");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("PhysicalEntity");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Placement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Date_Of_Construction")
                        .HasColumnType("integer");

                    b.Property<string>("House")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("PhysicalEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("Preriew_Image_Id")
                        .HasColumnType("integer");

                    b.Property<int>("Square")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("PhysicalEntityId");

                    b.ToTable("Placement");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("RentalOfPremises.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Deal", b =>
                {
                    b.HasOne("RentalOfPremises.Models.PhysicalEntity", "Owner")
                        .WithMany("OwnerDeals")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("RentalOfPremises.Models.Placement", "Placement")
                        .WithOne("Deal")
                        .HasForeignKey("RentalOfPremises.Models.Deal", "PlacementId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("RentalOfPremises.Models.PhysicalEntity", "Renter")
                        .WithMany("RentalDeals")
                        .HasForeignKey("RenterId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Placement");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Image", b =>
                {
                    b.HasOne("RentalOfPremises.Models.Placement", "Placement")
                        .WithMany("Images")
                        .HasForeignKey("PlacementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Placement");
                });

            modelBuilder.Entity("RentalOfPremises.Models.PhysicalEntity", b =>
                {
                    b.HasOne("RentalOfPremises.Models.User", "User")
                        .WithOne("PhysicalEntity")
                        .HasForeignKey("RentalOfPremises.Models.PhysicalEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Placement", b =>
                {
                    b.HasOne("RentalOfPremises.Models.PhysicalEntity", "PhysicalEntity")
                        .WithMany("Placements")
                        .HasForeignKey("PhysicalEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhysicalEntity");
                });

            modelBuilder.Entity("RentalOfPremises.Models.User", b =>
                {
                    b.HasOne("RentalOfPremises.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RentalOfPremises.Models.PhysicalEntity", b =>
                {
                    b.Navigation("OwnerDeals");

                    b.Navigation("Placements");

                    b.Navigation("RentalDeals");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Placement", b =>
                {
                    b.Navigation("Deal");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("RentalOfPremises.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("RentalOfPremises.Models.User", b =>
                {
                    b.Navigation("PhysicalEntity")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
