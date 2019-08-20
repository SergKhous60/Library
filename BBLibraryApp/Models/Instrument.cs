using BBLibraryApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("Instrument", Schema = "BigBand")]
    public class Instrument : EntityBase
    {
        [Display(Name = "Score Order")]
        public int ScoreOrder { get; set; }

        [Display(Name = "Default Set")]
        public bool IsInDefaultSet { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        [Display(Name = "Title"), Required]
        public string InstrumentName { get; set; }

        public ICollection<PersonInstrument> PeoplePlayList { get; set; }
        public ICollection<ChartInstrument> ChartsList { get; set; }
    }
}
