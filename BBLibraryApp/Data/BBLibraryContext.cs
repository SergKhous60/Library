using BBLibraryApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Data
{
    public class BBLibraryContext : DbContext
    {
        public BBLibraryContext() { }

        public BBLibraryContext(DbContextOptions<BBLibraryContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Chart> Charts { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<PerformanceChart> PerformanceCharts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonInstrument>()
                .HasKey(x => new { x.PersonID, x.InstrumentID });

            modelBuilder.Entity<PersonInstrument>()
                .HasOne(pi => pi.Person)
                .WithMany(p => p.SkillsSetInstruments)
                .HasForeignKey(pi => pi.PersonID);

            modelBuilder.Entity<PersonInstrument>()
                .HasOne(pi => pi.Instrument)
                .WithMany(i => i.PeoplePlayList)
                .HasForeignKey(pi => pi.InstrumentID);

            modelBuilder.Entity<ChartInstrument>()
                .HasKey(x => new { x.ChartID, x.InstrumentID });

            modelBuilder.Entity<ChartInstrument>()
                .HasOne(ci => ci.Chart)
                .WithMany(c => c.Instrumentation)
                .HasForeignKey(ci => ci.ChartID);

            modelBuilder.Entity<ChartInstrument>()
                .HasOne(ci => ci.Instrument)
                .WithMany(i => i.ChartsList)
                .HasForeignKey(ci => ci.InstrumentID);

            modelBuilder.Entity<PerformanceChart>()
                .HasOne(pc => pc.Performance)
                .WithMany(p => p.ConcertProgramme)
                .HasForeignKey(pc => pc.PerformanceID);

            modelBuilder.Entity<PerformanceChart>()
                .HasOne(pc => pc.Chart)
                .WithMany(c => c.ConcertsList)
                .HasForeignKey(pc => pc.ChartID);

            modelBuilder.Entity<Venue>()
                .HasIndex(v => v.VenueName)
                .IsUnique();

            modelBuilder.Entity<Performance>()
                .HasIndex(p => new { p.Name, p.Date })
                .IsUnique();

            modelBuilder.Entity<Chart>()
                .Property("Minutes").HasDefaultValueSql("0");

            modelBuilder.Entity<Chart>()
                .Property("Seconds").HasDefaultValueSql("0");
        }
    }
}
