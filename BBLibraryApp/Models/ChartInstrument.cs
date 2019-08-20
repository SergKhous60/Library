using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("ChartInstrument", Schema = "BigBand")]
    public class ChartInstrument
    {
        public int ChartID { get; set; }
        [ForeignKey(nameof(ChartID))]
        public Chart Chart { get; set; }

        public int InstrumentID { get; set; }
        [ForeignKey(nameof(InstrumentID))]
        public Instrument Instrument { get; set; }
    }
}
