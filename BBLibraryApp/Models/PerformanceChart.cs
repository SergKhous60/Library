using BBLibraryApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("PerformanceChart", Schema = "BigBand")]
    public class PerformanceChart : EntityBase
    {
        public int PerformanceID { get; set; }
        [ForeignKey(nameof(PerformanceID))]
        public Performance Performance { get; set; }

        public int ChartID { get; set; }
        [ForeignKey(nameof(ChartID))]
        public Chart Chart { get; set; }
        public int? ChartListOrder { get; set; }

        public int? PersonID { get; set; }
        [ForeignKey(nameof(PersonID))]
        public Person Person { get; set; }

        public int? InstrumentID { get; set; }
        [ForeignKey(nameof(InstrumentID))]
        public Instrument Instrument { get; set; }
    }
}
