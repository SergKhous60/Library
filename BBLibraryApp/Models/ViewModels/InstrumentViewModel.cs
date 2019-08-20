using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Models.ViewModels
{
    public class InstrumentViewModel
    {
        public int InstrumentID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
        public int ScoreOrder { get; set; }
    }
}
