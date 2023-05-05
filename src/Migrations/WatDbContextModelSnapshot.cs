﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using src.Data;

#nullable disable

namespace src.Migrations
{
    [DbContext(typeof(WatDbContext))]
    partial class WatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.3.23174.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Location", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .HasColumnType("longtext");

                    b.Property<bool>("isPlaceToEat")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("isPlaceToGetFood")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("name")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("LunchTime", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("day")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("fk_location")
                        .HasColumnType("int");

                    b.Property<int?>("fk_user")
                        .HasColumnType("int");

                    b.Property<bool>("participating")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("id");

                    b.ToTable("LunchTime");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .HasColumnType("longtext");

                    b.Property<int?>("fk_defaultPlaceToEat")
                        .HasColumnType("int");

                    b.Property<int?>("fk_defaultPlaceToGetFood")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("preferredLunchTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
