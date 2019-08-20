using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class ArtistsListInfo
    {
        public Chart Chart { get; set; }
        public int PerformanceID { get; set; }
        public ICollection<InstrumentPerson> InstrumentPeopleInfo { get; set; }
        public ICollection<Person> People { get; set; }
    }

    public class InstrumentPerson
    {
        public Instrument Instrument { get; set; }
        public Person Person { get; set; }
        public int ScoreOrder { get; set; }
    }
}
