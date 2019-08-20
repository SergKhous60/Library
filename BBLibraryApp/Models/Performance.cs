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
    [Table("Performance", Schema = "BigBand")]
    public class Performance : EntityBase
    {
        [Required, Display(Name = "Concert Name")]
        [DataType(DataType.Text), StringLength(100, MinimumLength = 1,
                    ErrorMessage = "Performance Name cannot be longer than 100 characters.")]
        [RegularExpression(@"^(\w+'?\w+ ?)*$",
                    ErrorMessage = "Please, do not start with white spaces and use alphanumeric characters and underscore." +
                    " You can use (') in the middle of the word.")]
        public string Name { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Date), Display(Name = "Performance Date")]
        public DateTime Date { get; set; }

        [DataType(DataType.Text), MaxLength(200)]
        public string Comments { get; set; }

        public string ShortComments
        {
            get { return Comments.TruncateString(25); }
        }


        public int VenueID { get; set; }
        [ForeignKey(nameof(VenueID))]
        public Venue Venue { get; set; }

        public ICollection<PerformanceChart> ConcertProgramme { get; set; }
    }
    
}
