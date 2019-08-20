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
    [Table("Venue", Schema = "BigBand")]
    public class Venue : EntityBase
    {
        [DataType(DataType.Text), MaxLength(150)]
        [Display(Name = "Venue Name"), RegularExpression(@"^(\w+ ?)*$",
            ErrorMessage = "Please, do not start with white spaces and use alphanumeric characters and underscore.")]
        public string VenueName { get; set; }

        [DataType(DataType.Text), MaxLength(50)]
        public string City { get; set; }

        [DataType(DataType.Text), MaxLength(200)]
        public string Contact { get; set; }

        [DataType(DataType.Text), MaxLength(200)]
        public string Comments { get; set; }

        public string ShortComments
        {
            //Showing only 40 characters in a table cell
            get { return Comments.TruncateString(40); }
        }

        [InverseProperty(nameof(Performance.Venue))]
        public ICollection<Performance> Performances { get; set; }
    }
}
