using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class InstrumentPersonInfo
    {
        public IEnumerable<Instrument> PersonInstruments { get; set; }
        public Chart Chart { get; set; }
        public int PerformanceID { get; set; }
    }
}
