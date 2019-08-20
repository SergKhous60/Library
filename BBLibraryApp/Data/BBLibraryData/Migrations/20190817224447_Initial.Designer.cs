﻿// <auto-generated />
using System;
using BBLibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBLibraryApp.Data.BBLibraryData.Migrations
{
    [DbContext(typeof(BBLibraryContext))]
    [Migration("20190817224447_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BBLibraryApp.Models.Chart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Arranger")
                        .HasMaxLength(50);

                    b.Property<string>("ChartName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Composer")
                        .HasMaxLength(50);

                    b.Property<int>("Minutes")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Note")
                        .HasMaxLength(150);

                    b.Property<string>("RecordingUrl")
                        .HasMaxLength(150);

                    b.Property<int>("Seconds")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int>("ShelfNumber");

                    b.HasKey("ID");

                    b.ToTable("Chart","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.ChartInstrument", b =>
                {
                    b.Property<int>("ChartID");

                    b.Property<int>("InstrumentID");

                    b.HasKey("ChartID", "InstrumentID");

                    b.HasIndex("InstrumentID");

                    b.ToTable("ChartInstrument","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.Instrument", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InstrumentName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("IsInDefaultSet");

                    b.Property<int>("ScoreOrder");

                    b.HasKey("ID");

                    b.ToTable("Instrument","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.Performance", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("VenueID");

                    b.HasKey("ID");

                    b.HasIndex("VenueID");

                    b.HasIndex("Name", "Date")
                        .IsUnique();

                    b.ToTable("Performance","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.PerformanceChart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChartID");

                    b.Property<int?>("ChartListOrder");

                    b.Property<int?>("InstrumentID");

                    b.Property<int>("PerformanceID");

                    b.Property<int?>("PersonID");

                    b.HasKey("ID");

                    b.HasIndex("ChartID");

                    b.HasIndex("InstrumentID");

                    b.HasIndex("PerformanceID");

                    b.HasIndex("PersonID");

                    b.ToTable("PerformanceChart","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.Person", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(200);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<bool>("IsFullTime");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<string>("Note")
                        .HasMaxLength(200);

                    b.Property<string>("Phone")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Person","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.PersonInstrument", b =>
                {
                    b.Property<int>("PersonID");

                    b.Property<int>("InstrumentID");

                    b.HasKey("PersonID", "InstrumentID");

                    b.HasIndex("InstrumentID");

                    b.ToTable("PersonInstrument","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.Venue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<string>("Comments")
                        .HasMaxLength(200);

                    b.Property<string>("Contact")
                        .HasMaxLength(200);

                    b.Property<string>("VenueName")
                        .HasMaxLength(150);

                    b.HasKey("ID");

                    b.HasIndex("VenueName")
                        .IsUnique()
                        .HasFilter("[VenueName] IS NOT NULL");

                    b.ToTable("Venue","BigBand");
                });

            modelBuilder.Entity("BBLibraryApp.Models.ChartInstrument", b =>
                {
                    b.HasOne("BBLibraryApp.Models.Chart", "Chart")
                        .WithMany("Instrumentation")
                        .HasForeignKey("ChartID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BBLibraryApp.Models.Instrument", "Instrument")
                        .WithMany("ChartsList")
                        .HasForeignKey("InstrumentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BBLibraryApp.Models.Performance", b =>
                {
                    b.HasOne("BBLibraryApp.Models.Venue", "Venue")
                        .WithMany("Performances")
                        .HasForeignKey("VenueID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BBLibraryApp.Models.PerformanceChart", b =>
                {
                    b.HasOne("BBLibraryApp.Models.Chart", "Chart")
                        .WithMany("ConcertsList")
                        .HasForeignKey("ChartID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BBLibraryApp.Models.Instrument", "Instrument")
                        .WithMany()
                        .HasForeignKey("InstrumentID");

                    b.HasOne("BBLibraryApp.Models.Performance", "Performance")
                        .WithMany("ConcertProgramme")
                        .HasForeignKey("PerformanceID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BBLibraryApp.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonID");
                });

            modelBuilder.Entity("BBLibraryApp.Models.PersonInstrument", b =>
                {
                    b.HasOne("BBLibraryApp.Models.Instrument", "Instrument")
                        .WithMany("PeoplePlayList")
                        .HasForeignKey("InstrumentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BBLibraryApp.Models.Person", "Person")
                        .WithMany("SkillsSetInstruments")
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
