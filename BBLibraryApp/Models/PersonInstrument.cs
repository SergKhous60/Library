using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("PersonInstrument", Schema = "BigBand")]
    public class PersonInstrument
    {
        public int PersonID { get; set; }
        [ForeignKey(nameof(PersonID))]
        public Person Person { get; set; }

        public int InstrumentID { get; set; }
        [ForeignKey(nameof(InstrumentID))]
        public Instrument Instrument { get; set; }
    }
}
