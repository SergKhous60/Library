using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class ChartDetailsData
    {
        public Chart Chart { get; set; }
        public IEnumerable<Instrument> Instruments { get; set; }
    }
}
