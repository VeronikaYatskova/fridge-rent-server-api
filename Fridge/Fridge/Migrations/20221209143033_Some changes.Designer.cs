﻿// <auto-generated />
using System;
using Fridge.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fridge.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20221209143033_Some changes")]
    partial class Somechanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Fridge.Data.Models.Fridge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("FridgeId");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProducerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RentDocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RenterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProducerId");

                    b.HasIndex("RenterId");

                    b.ToTable("Fridges");
                });

            modelBuilder.Entity("Fridge.Data.Models.FridgeProduct", b =>
                {
                    b.Property<Guid>("FridgeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FridgeId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("FridgeProducts");
                });

            modelBuilder.Entity("Fridge.Data.Models.Model", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("ModelName");

                    b.HasKey("Id");

                    b.ToTable("Models");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f8a3b786-d4b2-49d7-953b-578729b55a35"),
                            Name = "Indesit ITR 5200 W"
                        },
                        new
                        {
                            Id = new Guid("4a645006-5621-4536-9490-e1769fac2f53"),
                            Name = "LG GA-B379SLUL"
                        },
                        new
                        {
                            Id = new Guid("44dc042a-3453-4c17-a4d1-cd8c0ac9378c"),
                            Name = "ATLANT XM-4208-000"
                        },
                        new
                        {
                            Id = new Guid("2182354c-d8cc-47bf-844f-4aafaba1dbfe"),
                            Name = "ATLANT ХМ 4625-101 NL"
                        },
                        new
                        {
                            Id = new Guid("af96137e-0b17-41b5-a819-a5a23da0fd97"),
                            Name = "Toshiba GR-RF610WE-PMS"
                        });
                });

            modelBuilder.Entity("Fridge.Data.Models.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("OwnerId");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("Fridge.Data.Models.Producer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProducerId");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("ProducerCountry");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("ProducerName");

                    b.HasKey("Id");

                    b.ToTable("Producer");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d347dfe3-5cf9-49e8-8137-8880580f203b"),
                            Country = "Belarus",
                            Name = "ATLANT"
                        },
                        new
                        {
                            Id = new Guid("a8000178-a46b-4122-8758-2931e99c46e9"),
                            Country = "Russia",
                            Name = "Indesit"
                        },
                        new
                        {
                            Id = new Guid("38886c70-4593-47ce-9cd1-99d9831c2eb4"),
                            Country = "Russia",
                            Name = "LG"
                        },
                        new
                        {
                            Id = new Guid("0d08c561-361c-497e-bd21-06a7ce7d5516"),
                            Country = "China",
                            Name = "Toshiba"
                        },
                        new
                        {
                            Id = new Guid("8e652090-8fa2-4271-8e05-7934a0ba77a7"),
                            Country = "Russia",
                            Name = "BEKO"
                        });
                });

            modelBuilder.Entity("Fridge.Data.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProductId");

                    b.Property<int>("DefaultQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("ProductName");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("36b74198-a896-429f-b040-0512fca189a8"),
                            DefaultQuantity = 3,
                            Name = "Apple"
                        },
                        new
                        {
                            Id = new Guid("f2ddea9c-7691-4c7a-99ec-abaec36db9bd"),
                            DefaultQuantity = 1,
                            Name = "Milk"
                        },
                        new
                        {
                            Id = new Guid("fdb08eb6-d113-4d8a-8576-3454bb89ad55"),
                            DefaultQuantity = 10,
                            Name = "Eggs"
                        },
                        new
                        {
                            Id = new Guid("b89aa809-9fac-4c67-af3b-6599ade45f92"),
                            DefaultQuantity = 1,
                            Name = "Cake"
                        },
                        new
                        {
                            Id = new Guid("3ab533b7-2ae5-4121-85dd-9d977e1b53ed"),
                            DefaultQuantity = 5,
                            Name = "Tomato"
                        });
                });

            modelBuilder.Entity("Fridge.Data.Models.ProductPicture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RenterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RenterId");

                    b.ToTable("ProductPictures");
                });

            modelBuilder.Entity("Fridge.Data.Models.RentDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FridgeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("MonthCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("RenterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FridgeId")
                        .IsUnique();

                    b.HasIndex("RenterId");

                    b.ToTable("RentDocuments");
                });

            modelBuilder.Entity("Fridge.Data.Models.Renter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Renters");
                });

            modelBuilder.Entity("Fridge.Data.Models.Fridge", b =>
                {
                    b.HasOne("Fridge.Data.Models.Model", "Model")
                        .WithMany("Fridges")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Owner", "Owner")
                        .WithMany("Fridges")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Producer", "Producer")
                        .WithMany("Fridges")
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Renter", "Renter")
                        .WithMany("Fridges")
                        .HasForeignKey("RenterId");

                    b.Navigation("Model");

                    b.Navigation("Owner");

                    b.Navigation("Producer");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("Fridge.Data.Models.FridgeProduct", b =>
                {
                    b.HasOne("Fridge.Data.Models.Fridge", "Fridge")
                        .WithMany()
                        .HasForeignKey("FridgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fridge");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Fridge.Data.Models.ProductPicture", b =>
                {
                    b.HasOne("Fridge.Data.Models.Product", "Product")
                        .WithMany("ProductPictures")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Renter", "Renter")
                        .WithMany("ProductPictures")
                        .HasForeignKey("RenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("Fridge.Data.Models.RentDocument", b =>
                {
                    b.HasOne("Fridge.Data.Models.Fridge", "Fridge")
                        .WithOne("RentDocument")
                        .HasForeignKey("Fridge.Data.Models.RentDocument", "FridgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fridge.Data.Models.Renter", "Renter")
                        .WithMany("RentDocuments")
                        .HasForeignKey("RenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fridge");

                    b.Navigation("Renter");
                });

            modelBuilder.Entity("Fridge.Data.Models.Fridge", b =>
                {
                    b.Navigation("RentDocument");
                });

            modelBuilder.Entity("Fridge.Data.Models.Model", b =>
                {
                    b.Navigation("Fridges");
                });

            modelBuilder.Entity("Fridge.Data.Models.Owner", b =>
                {
                    b.Navigation("Fridges");
                });

            modelBuilder.Entity("Fridge.Data.Models.Producer", b =>
                {
                    b.Navigation("Fridges");
                });

            modelBuilder.Entity("Fridge.Data.Models.Product", b =>
                {
                    b.Navigation("ProductPictures");
                });

            modelBuilder.Entity("Fridge.Data.Models.Renter", b =>
                {
                    b.Navigation("Fridges");

                    b.Navigation("ProductPictures");

                    b.Navigation("RentDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
