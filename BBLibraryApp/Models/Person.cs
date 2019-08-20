using BBLibraryApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models
{
    [Table("Person", Schema = "BigBand")]
    public class Person : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName
        {
            get { return $"{LastName } {FirstName}"; }
        }

        [Display(Name = "First Name")]
        [DataType(DataType.Text), MaxLength(50)]
        public string FirstName { get; set; }

        [DataType(DataType.Text), MaxLength(200)]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress), MaxLength(100)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber), MaxLength(50)]
        public string Phone { get; set; }

        [DataType(DataType.Text), MaxLength(200)]
        public string Note { get; set; }

        [Display(Name = "Is Full Time?")]
        public bool IsFullTime { get; set; }

        public ICollection<PersonInstrument> SkillsSetInstruments { get; set; }
    }
}
