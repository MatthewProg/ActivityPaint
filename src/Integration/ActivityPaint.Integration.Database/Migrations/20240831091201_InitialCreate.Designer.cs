﻿// <auto-generated />
using System;
using ActivityPaint.Integration.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ActivityPaint.Integration.Database.Migrations
{
    [DbContext(typeof(ActivityPaintContext))]
    [Migration("20240831091201_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("ActivityPaint.Core.Entities.Preset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CanvasData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDarkModeDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("ActivityPaint.Core.Entities.RepositoryConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorFullName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageFormat")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RepositoryConfigs");
                });
#pragma warning restore 612, 618
        }
    }
}
