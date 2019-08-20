using BBLibraryApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BBLibraryContext context)
        {
            context.Database.Migrate();

            // Look for any instruments.
            if (context.Instruments.Any())
            {
                return; // Db has been seeded
            }

            var instruments = new Instrument[]
            {
                new Instrument{InstrumentName="Conductor",IsInDefaultSet=true,ScoreOrder=1},
                new Instrument{InstrumentName="1st Eb Alto Saxophone",IsInDefaultSet=true,ScoreOrder=2},
                new Instrument{InstrumentName="2nd Eb Alto Saxophone",IsInDefaultSet=true,ScoreOrder=3},
                new Instrument{InstrumentName="1st Bb Tenor Saxophone",IsInDefaultSet=true,ScoreOrder=4},
                new Instrument{InstrumentName="2nd Bb Tenor Saxophone",IsInDefaultSet=true,ScoreOrder=5},
                new Instrument{InstrumentName="Eb Baritone Saxophone",IsInDefaultSet=true,ScoreOrder=6},
                new Instrument{InstrumentName="1st Bb Trumpet",IsInDefaultSet=true,ScoreOrder=7},
                new Instrument{InstrumentName="2nd Bb Trumpet",IsInDefaultSet=true,ScoreOrder=8},
                new Instrument{InstrumentName="3rd Bb Trumpet",IsInDefaultSet=true,ScoreOrder=9},
                new Instrument{InstrumentName="4th Bb Trumpet",IsInDefaultSet=true,ScoreOrder=10},
                new Instrument{InstrumentName="1st Trombone",IsInDefaultSet=true,ScoreOrder=11},
                new Instrument{InstrumentName="2nd Trombone",IsInDefaultSet=true,ScoreOrder=12},
                new Instrument{InstrumentName="3rd Trombone",IsInDefaultSet=true,ScoreOrder=13},
                new Instrument{InstrumentName="4th Trombone",IsInDefaultSet=true,ScoreOrder=14},
                new Instrument{InstrumentName="Guitar",IsInDefaultSet=true,ScoreOrder=15},
                new Instrument{InstrumentName="Piano",IsInDefaultSet=true,ScoreOrder=16},
                new Instrument{InstrumentName="Bass",IsInDefaultSet=true,ScoreOrder=17},
                new Instrument{InstrumentName="Drums",IsInDefaultSet=true,ScoreOrder=18}
            };
            foreach (Instrument inst in instruments)
            {
                context.Instruments.Add(inst);
            }
            //context.SaveChanges();

            var venues = new Venue[]
            {
                new Venue{VenueName = "Bruce Mason Centre", City = "Auckland", Contact = "09-488 2940", Comments = "Total Capacity: 1022"},
                new Venue{VenueName = "Auckland Town Hall", City = "Auckland", Contact = "09-307 5498", Comments = "Total Capacity: 1,529"}
            };
            foreach (var venue in venues)
            {
                context.Venues.Add(venue);
            }
            //context.SaveChanges();

            Random r = new Random();

            for (int i = 1; i <= 500; i++)
            {
                var newChart= new Chart
                {
                    ChartName = $"Chart Name {i.ToString()}",
                    Arranger = $"Arranger {i.ToString()}",
                    Composer = $"Composer {i.ToString()}",
                    Minutes = r.Next(0, 5),
                    Seconds = r.Next(0, 60),
                    ShelfNumber = i,
                };
                context.Charts.Add(newChart);
            }
            //context.SaveChanges();

            var artists = new Person[]
            {
                new Person{LastName="<None>"},
                new Person{LastName="Doe",FirstName="John",IsFullTime=true},
                new Person{LastName="Doe",FirstName="Jane",IsFullTime=false},
                new Person{LastName="Nurk",FirstName="Fred",IsFullTime=true},
                new Person{LastName="Bloggs",FirstName="Joe",IsFullTime=true}
            };
            context.AddRange(artists);
            context.SaveChanges();
        }
    }
}
