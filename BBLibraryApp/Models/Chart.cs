using BBLibraryApp.Infrastructure;
using BBLibraryApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("Chart", Schema = "BigBand")]
    public class Chart : EntityBase
    {
        [Display(Name = "Title"), Required]
        [DataType(DataType.Text), StringLength(150, ErrorMessage =
            "Chart Name cannot be longer than 150 characters.")]
        public string ChartName { get; set; }

        public string ShortChartName
        {
            get { return ChartName.TruncateString(40); }
        }

        [Range(0, 59)]
        public int Minutes { get; set; }
        [Range(0, 59)]
        public int Seconds { get; set; }

        public int Time
        {
            get { return Minutes * 60 + Seconds; }
        }

        public string Length
        {
            get
            {
                string min = Minutes.ToString();
                string sec = Seconds.ToString();
                if (Seconds < 10)
                {
                    sec = "0" + Seconds.ToString();
                }
                if (Minutes < 10)
                {
                    min = "0" + Minutes.ToString();
                }
                return $"{min}:{sec}";
            }
        }

        [DataType(DataType.Text), MaxLength(50)]
        public string Composer { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        public string Arranger { get; set; }

        [DataType(DataType.Text), MaxLength(150)]
        [Display(Name = "Recording")]
        public string RecordingUrl { get; set; }

        [DataType(DataType.Text), MaxLength(150)]
        public string Note { get; set; }

        public string ShortNote
        {
            //Showing only 40 characters in a table cell
            get { return Note.TruncateString(40); }
        }

        [Display(Name = "Catalogue")]
        public int ShelfNumber { get; set; }

        public string Catalogue { get { return $"BB{ShelfNumber}"; } }

        public ICollection<ChartInstrument> Instrumentation { get; set; }
        public ICollection<PerformanceChart> ConcertsList { get; set; }

    }
}
